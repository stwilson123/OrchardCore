using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Application.Abstratctions.Controller.Builder;
using BlocksCore.Application.Abstratctions.Controller.Factory;
using BlocksCore.Application.Abstratctions.Filters;
using BlocksCore.SyntacticAbstractions.Types;
using RemoteServiceAttribute = BlocksCore.Application.Abstratctions.Attributes.RemoteServiceAttribute;

namespace BlocksCore.Application.Core.Controller.Builder
{
    public class BatchDefaultControllerBuilder<T> : IBatchDefaultControllerBuilder<T>
    {
        private string _servicePrefix;
       // private readonly IIocManager _iocManager;
        private readonly IDefaultControllerBuilderFactory _controllerBuilderFactory;
        private readonly IEnumerable<Type> _types;

        protected  virtual string[] ControllerPostfixes { get { return AppService.Postfixes;  } }
        private Action<IDefaultControllerActionBuilder> _forMethodsAction;
        private bool _conventionalVerbs;
        private IFilter[] _filters;
        private Func<Type, string> _serviceNameSelector;

        public BatchDefaultControllerBuilder(
           // IIocManager iocManager,
            IDefaultControllerBuilderFactory controllerBuilderFactory, 
            string servicePrefix,IEnumerable<Type> types)
        {
            Check.NotNull(types, "types");
           // _iocManager = iocManager;
            _controllerBuilderFactory = controllerBuilderFactory;
            _types = types;
            _servicePrefix = servicePrefix;
        }
        public IBatchDefaultControllerBuilder<T> ForMethods(Action<IDefaultControllerActionBuilder> action)
        {
            _forMethodsAction = action;
            return this;
        }

        public IBatchDefaultControllerBuilder<T> WithConventionalVerbs()
        {
            _conventionalVerbs = true;
            return this;
        }

        public IBatchDefaultControllerBuilder<T> WithFilters(params IFilter[] filters)
        {
            _filters = filters;
            return this;
        }
        
        public IBatchDefaultControllerBuilder<T> WithServiceName(Func<Type, string> serviceNameSelector)
        {
            _serviceNameSelector = serviceNameSelector;
            return this;
        }
        public void Build()
        {

            var types =
                _types
                    .Where(type => (type.IsPublic || type.IsNestedPublic)  &&
                                   typeof(T).IsAssignableFrom(type) &&
                                   !RemoteServiceAttribute.IsExplicitlyDisabledFor(type)
                    );

//            if (_typePredicate != null)
//            {
//                types = types.Where(t => _typePredicate(t));
//            }
 
            foreach (var type in types)
            {
                var serviceName = _serviceNameSelector != null
                    ? _serviceNameSelector(type)
                    : GetConventionalServiceName(type);

                if (!string.IsNullOrWhiteSpace(_servicePrefix))
                {
                    serviceName = _servicePrefix + "/" + serviceName;
                }
                var builder = typeof(IDefaultControllerBuilderFactory)
                    .GetMethod("For", BindingFlags.Public | BindingFlags.Instance)
                    .MakeGenericMethod(type)
                    .Invoke(_controllerBuilderFactory, new object[] { _servicePrefix, serviceName });

                if (_filters != null)
                {
                    builder.GetType()
                        .GetMethod("WithFilters", BindingFlags.Public | BindingFlags.Instance)
                        .Invoke(builder, new object[] {_filters});
                }

         
                if (_conventionalVerbs)
                {
                    builder.GetType()
                        .GetMethod("WithConventionalVerbs", BindingFlags.Public | BindingFlags.Instance)
                        .Invoke(builder, new object[0]);
                }

                if (_forMethodsAction != null)
                {
                    builder.GetType()
                        .GetMethod("ForMethods", BindingFlags.Public | BindingFlags.Instance)
                        .Invoke(builder, new object[] {_forMethodsAction});
                }

                builder.GetType()
                    .GetMethod("Build", BindingFlags.Public | BindingFlags.Instance)
                    .Invoke(builder, new object[0]);
            }
        }


        protected virtual string GetConventionalServiceName(Type type)
        {
            var typeName = type.Name;

            typeName = typeName.RemovePostFix(ControllerPostfixes);

            if (typeName.Length > 1 && typeName.StartsWith("I") && char.IsUpper(typeName, 1))
            {
                typeName = typeName.Substring(1);
            }

            return typeName;
        }
    }

   
}