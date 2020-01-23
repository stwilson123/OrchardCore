using System;
using System.Reflection;
using BlocksCore.Application.Abstratctions.Controller;
using BlocksCore.Application.Abstratctions.Controller.Builder;
using BlocksCore.Application.Abstratctions.Filters;
using BlocksCore.SyntacticAbstractions.Threading;
using BlocksCore.SyntacticAbstractions.Types;

namespace BlocksCore.Application.Core.Controller.Builder
{
    public class DefaultControllerActionBuilder<T> : IDefaultControllerActionBuilder<T>
    {
          /// <summary>
        /// Selected action name.
        /// </summary>
        public string ActionName { get; }

        /// <summary>
        /// Underlying proxying method.
        /// </summary>
        public MethodInfo Method { get; }

  

        /// <summary>
        /// Is API Explorer enabled.
        /// </summary>
        public bool? IsApiExplorerEnabled { get; set; }

        /// <summary>
        /// Action Filters for dynamic controller method.
        /// </summary>
        public IFilter[] Filters { get; set; }
    
        /// <summary>
        /// A flag to set if no action will be created for this method.
        /// </summary>
        public bool DontCreate { get; set; }


        /// <summary>
        /// Reference to the <see cref="ApiControllerBuilder{T}"/> which created this object.
        /// </summary>
        public IDefaultControllerBuilder Controller
        {
            get { return _controller; }
        }

     
        private readonly IDefaultControllerBuilder<T> _controller;

 
        /// <summary>
        /// Creates a new <see cref="defaultControllerBuilder{T}"/> object.
        /// </summary>
        /// <param name="defaultControllerBuilder">Reference to the <see cref="defaultControllerBuilder{T}"/> which created this object</param>
        /// <param name="methodInfo">Method</param>
        /// <param name="iocResolver"></param>
        public DefaultControllerActionBuilder(IDefaultControllerBuilder<T> defaultControllerBuilder, MethodInfo methodInfo)
        {
            _controller = defaultControllerBuilder;
            Method = methodInfo;
            ActionName = GetNormalizedActionName();
        }

       
        private string GetNormalizedActionName()
        {

            if (!Method.IsAsync())
            {
                return Method.Name;
            }

            return Method.Name.RemovePostFix("Async");
        }


        /// <summary>
        /// Enables/Disables API Explorer for the action.
        /// </summary>
        public IDefaultControllerActionBuilder<T> WithApiExplorer(bool isEnabled)
        {
            IsApiExplorerEnabled = isEnabled;
            return this;
        }

        /// <summary>
        /// Used to specify another method definition.
        /// </summary>
        /// <param name="methodName">Name of the method in proxied type</param>
        /// <returns>Action builder</returns>
        public IDefaultControllerActionBuilder<T> ForMethod(string methodName)
        {
            return _controller.GetMethod(methodName);
        }


        /// <summary>
        /// Used to add action filters to apply to this method.
        /// </summary>
        /// <param name="filters"> Action Filters to apply.</param>
        public IDefaultControllerActionBuilder<T> WithFilters(params IFilter[]  filters)
        {
            Filters = filters;
            return this;
        }
       

        /// <summary>
        /// Tells builder to not create action for this method.
        /// </summary>
        /// <returns>Controller builder</returns>
        public IDefaultControllerBuilder<T> DontCreateAction()
        {
            DontCreate = true;
            return _controller;
        }

        /// <summary>
        /// Builds the controller.
        /// This method must be called at last of the build operation.
        /// </summary>
        public virtual void  Build()
        {
            
        }

        public virtual DefaultControllerActionInfo GetResult()
        {
            return new DefaultControllerActionInfo(
                ActionName,
                Method,
                Filters,
                IsApiExplorerEnabled
            );
        }
    }
}