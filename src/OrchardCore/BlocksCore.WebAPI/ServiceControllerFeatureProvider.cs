using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlocksCore.Application.Abstratctions;
using BlocksCore.WebAPI.Controllers.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;




namespace BlocksCore.WebAPI.Providers
{
    internal class ServiceControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly MvcControllerManager _mvcControllerManager;
        private const string ControllerTypeNameSuffix = "Controller";


        public ServiceControllerFeatureProvider(MvcControllerManager mvcControllerManager)
        {
            _mvcControllerManager = mvcControllerManager;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var controllerInfo in _mvcControllerManager.GetAll())
            {
                feature.Controllers.Add(controllerInfo.ServiceInterfaceType.GetTypeInfo());
            }
 
        }

        protected bool IsController(Type typeInfo)
        {
            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            // We only consider public top-level classes as controllers. IsPublic returns false for nested
            // classes, regardless of visibility modifiers
            if (!typeInfo.IsPublic)
            {
                return false;
            }

            if (typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
            {
                return false;
            }

            if (!typeInfo.Name.EndsWith(ControllerTypeNameSuffix, StringComparison.OrdinalIgnoreCase) &&
                !typeInfo.IsDefined(typeof(ControllerAttribute)))
            {
                return false;
            }

            if (!typeof(IAppService).IsAssignableFrom(typeInfo))
                return false;
            return true;
        }
    }
}