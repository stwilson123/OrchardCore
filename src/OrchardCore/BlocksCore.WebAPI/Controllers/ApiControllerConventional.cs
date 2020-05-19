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
using System.Collections.Generic;
using OrchardCore.Environment.Extensions.Features;

namespace BlocksCore.WebAPI.Controllers
{
    /// <summary>
    /// Registers all MVC Controllers derived from <see cref="Controller"/>.
    /// </summary>
    public class ApiControllerConventional
    {
        private readonly IEnumerable<FeatureEntry> _features;
        private readonly MvcControllerBuilderFactory _mvcControllerBuilderFactory;

        internal static string GetControllerSerivceName(string area,string controllerName)
        {
            return $@"api/{area}/{controllerName}";
        }

        public  static  string[] Postfixes()
        {
            return new[] {"Service"};
        }

        internal ApiControllerConventional(IEnumerable<FeatureEntry> features, MvcControllerBuilderFactory mvcControllerBuilderFactory)
        {
            _features = features;
            _mvcControllerBuilderFactory = mvcControllerBuilderFactory;
        }

  
        internal void RegisterController()
        {

            var features = _features ?? Enumerable.Empty<FeatureEntry>();
            foreach (var feature in features)
            {
                var controllers = feature.ExportedTypes.Where(IsController);
                if (!controllers.Any())
                    continue;
                _mvcControllerBuilderFactory.ForAll<IAppService>( AreaTemplate.GetAreaKey(new AreaOption() { AreaName = feature.FeatureInfo.Id,
                     FunctionType = "api"
                }), controllers).Build();
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