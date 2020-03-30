using System;
using System.Collections.Generic;
using BlocksCore.Application.Abstratctions.Controller.Builder;
using BlocksCore.Application.Core.Controller.Factory;
using BlocksCore.SyntacticAbstractions.Types;
using BlocksCore.WebAPI.Controllers.Builder;
using BlocksCore.WebAPI.Controllers.Manager;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.WebAPI.Controllers.Factory
{
    public class MvcControllerBuilderFactory : DefaultControllerBuilderFactory
    {
        private readonly MvcControllerOption _mvcControllerOption;

        public MvcControllerBuilderFactory(MvcControllerOption mvcControllerOption,MvcControllerManager mvcControllerManager) : base(mvcControllerManager)
        {
            Check.NotNull(mvcControllerOption, nameof(mvcControllerOption));
            _mvcControllerOption = mvcControllerOption;
        }

        public override IDefaultControllerBuilder<T> For<T>(string servicePrefix, string serviceName)
        {
            return new MvcControllerBuilder<T,MvcControllerActionBuilder<T>>(servicePrefix, serviceName, this.defaultControllerManager
                ,_mvcControllerOption.ApiControllerType);
        }

        public override IBatchDefaultControllerBuilder<T> ForAll<T>(string servicePrefix,IEnumerable<Type> serviceTypes)
        {
            return new BatchMvcControllerBuilder<T>(this, servicePrefix,serviceTypes);
        }
    }
} 