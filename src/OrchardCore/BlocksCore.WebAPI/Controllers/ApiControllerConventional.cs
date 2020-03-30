using System;
using System.Linq;
using System.Reflection;
using BlocksCore.Abstractions;
using BlocksCore.Application.Abstratctions;
using BlocksCore.WebAPI.Controllers.Factory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using BlocksCore.SyntacticAbstractions.Types;
using OrchardCore.Environment.Extensions;
using BlocksCore.Web.Abstractions;

namespace BlocksCore.WebAPI.Controllers
{
    /// <summary>
    /// Registers all MVC Controllers derived from <see cref="Controller"/>.
    /// </summary>
    public class ApiControllerConventional
    {
        private readonly IExtensionManager _extensionManager;
        private readonly MvcControllerBuilderFactory _mvcControllerBuilderFactory;

        internal static string GetControllerSerivceName(string area,string controllerName)
        {
            return $@"api/{area}/{controllerName}";
        }

        public  static  string[] Postfixes()
        {
            return new[] {"Service"};
        }

        internal ApiControllerConventional(IExtensionManager extensionManager,MvcControllerBuilderFactory mvcControllerBuilderFactory)
        {
            _extensionManager = extensionManager;
            _mvcControllerBuilderFactory = mvcControllerBuilderFactory;
        }

  
        internal void RegisterController()
        {

            var features = _extensionManager.LoadFeaturesAsync().Result;
            foreach (var feature in features)
            {
                _mvcControllerBuilderFactory.ForAll<IAppService>( AreaTemplate.GetAreaKey(new AreaOption() { AreaName = feature.FeatureInfo.Id,
                     FunctionType = "api"
                }),feature.ExportedTypes.Where(IsController)).Build();
            }
          
        }

        private bool IsController(Type typeInfo)
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

            if (!typeInfo.Name.EndsWith(others:Postfixes() ,(name, fixes) => fixes.All(f => f) ) &&
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