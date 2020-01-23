using System;
using System.Collections.Generic;
using System.Linq;
using BlocksCore.Application.Abstratctions.Attributes;
using BlocksCore.Application.Abstratctions.Controller;
using BlocksCore.Application.Abstratctions.Controller.Attributes;
using BlocksCore.Application.Abstratctions.Controller.Builder;
using BlocksCore.Application.Abstratctions.Controller.Helper;
using BlocksCore.Application.Abstratctions.Filters;
using BlocksCore.Application.Abstratctions.Manager;
using BlocksCore.Exception;
using BlocksCore.SyntacticAbstractions.Reflection.Extensions;
using BlocksCore.SyntacticAbstractions.Types;

namespace BlocksCore.Application.Core.Controller.Builder
{
    public class DefaultControllerBuilder<T,TControllerActionBuilder> : IDefaultControllerBuilder<T> where TControllerActionBuilder :DefaultControllerActionBuilder<T>  
    {
        protected Dictionary<string, TControllerActionBuilder> _actionBuilders;

        public string ServicePrefix { get; }

        public string ServiceName { get; }
        public Type ServiceInterfaceType { get; set; }
        public bool? IsApiExplorerEnabled { get; set; }

        public IFilter[] Filters { set; get; }
        protected IControllerRegister _defaultControllerManager;

        protected virtual Type ApiControllerType
        {
            get { return typeof(NopController); }
        }

        /// <summary>
        /// Creates a new instance of ApiControllerInfoBuilder.
        /// </summary>
        /// <param name="serviceName">Name of the controller</param>
        /// <param name="iocResolver">Ioc resolver</param>
        public DefaultControllerBuilder(string servicePrefix, string serviceName, IControllerRegister defaultControllerManager)
        {

            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new ArgumentException("serviceName null or empty!", "serviceName");
            }

            if (!DynamicApiServiceNameHelper.IsValidServiceName(serviceName))
            {
                throw new ArgumentException("serviceName is not properly formatted! It must contain a single-depth namespace at least! For example: 'myapplication/myservice'.", "serviceName");
            }

            _defaultControllerManager = defaultControllerManager;
            ServicePrefix = servicePrefix;
            ServiceName = serviceName;
            ServiceInterfaceType = typeof (T);

            _actionBuilders = new Dictionary<string, TControllerActionBuilder>();
            var methodInfos = DynamicApiControllerActionHelper.GetMethodsOfType(typeof(T))
                .Where(methodInfo => methodInfo.GetSingleAttributeOrNull<BlocksActionNameAttribute>() != null);
            foreach (var methodInfo in methodInfos)
            {
                var actionBuilder = (TControllerActionBuilder)typeof(TControllerActionBuilder).New(this, methodInfo);
                var remoteServiceAttr = methodInfo.GetSingleAttributeOrNull<RemoteServiceAttribute>();
                if (remoteServiceAttr != null && !remoteServiceAttr.IsEnabledFor(methodInfo))
                {
                    actionBuilder.DontCreateAction();
                }
                var actionNameAttr = methodInfo.GetSingleAttributeOrNull<BlocksActionNameAttribute>();
 

                _actionBuilders[actionNameAttr.ActionName] =
                    actionBuilder;
            }
        }
        
        
        
        public IDefaultControllerBuilder<T> WithApiExplorer(bool isEnabled)
        {
            IsApiExplorerEnabled = isEnabled;
            return this;
        }
      
        
     
        public IDefaultControllerActionBuilder<T> GetMethod(string methodName)
        {
            if (!_actionBuilders.ContainsKey(methodName))
            {
                throw new BlocksException($"There is no method with name " + methodName + " in type " + typeof(T).Name);
            }

            return _actionBuilders[methodName];
        }
        public IDefaultControllerBuilder<T> ForMethods(Action<IDefaultControllerActionBuilder> action)
        {
            foreach (var actionBuilder in _actionBuilders.Values)
            {
                action(actionBuilder);
            }

            return this;
        }
        public virtual void Build()
        {
            var controllerInfo = new DefaultControllerInfo<DefaultControllerActionInfo>(
                ServicePrefix,
                ServiceName,
                ServiceInterfaceType,
                ApiControllerType,
                null, //TODO
                Filters,
                IsApiExplorerEnabled
            );
            
            foreach (var actionBuilder in _actionBuilders.Values)
            {
                if (actionBuilder.DontCreate)
                {
                    continue;
                }
                actionBuilder.Build();
                controllerInfo.Actions[actionBuilder.ActionName] = actionBuilder.GetResult();
            }

            _defaultControllerManager.Register(controllerInfo);
        }

    }
}