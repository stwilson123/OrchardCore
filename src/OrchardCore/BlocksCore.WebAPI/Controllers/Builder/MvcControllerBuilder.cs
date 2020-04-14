using System;
using BlocksCore.Application.Abstratctions.Controller;
using BlocksCore.Application.Abstratctions.Controller.Builder;
using BlocksCore.Application.Abstratctions.Manager;
using BlocksCore.Application.Core.Controller.Builder;

namespace BlocksCore.WebAPI.Controllers.Builder
{
    public class MvcControllerBuilder<T,TControllerActionBuilder> : DefaultControllerBuilder<T,TControllerActionBuilder> where TControllerActionBuilder : MvcControllerActionBuilder<T>
    {
        public bool ConventionalVerbs { get; set; }

        private Type apiControllerType;

        protected override Type ApiControllerType
        {
            get
            {
                return this.apiControllerType ?? base.ApiControllerType;
            }
        }

        public MvcControllerBuilder(string servicePrefix,string serviceName, IControllerRegister defaultControllerManager, Type apiControllerType) : base(servicePrefix,serviceName, defaultControllerManager)
        {
            this.apiControllerType = apiControllerType;
        }

        public IDefaultControllerBuilder<T> WithConventionalVerbs()
        {
            ConventionalVerbs = true;
            return this;
        }
        public override void Build()
        {
            var controllerInfo = new DefaultControllerInfo<MvcControllerActionInfo>(
                ServicePrefix,
                ServiceName,
                ServiceType,
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
                controllerInfo.Actions[actionBuilder.ActionName] = actionBuilder.GetResult() as MvcControllerActionInfo;
            }

            _defaultControllerManager.Register(controllerInfo);
        }
    }
}