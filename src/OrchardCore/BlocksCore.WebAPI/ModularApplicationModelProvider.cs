using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlocksCore.Application.Abstratctions;
using BlocksCore.WebAPI.Controllers.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Hosting;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Descriptor.Models;
using OrchardCore.Environment.Shell.Models;

namespace BlocksCore.WebAPI
{
    /// <summary>
    /// Adds an area route constraint using the name of the module.
    /// And filters all controller actions of disabled features.
    /// </summary>
    public class ModularApplicationModelProvider : IApplicationModelProvider
    {
        private readonly ITypeFeatureProvider _typeFeatureProvider;
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly ShellSettings _shellSettings;
        private readonly MvcControllerManager _defaultMvcControllerManager;

        public ModularApplicationModelProvider(
            ITypeFeatureProvider typeFeatureProvider,
            IHostEnvironment hostingEnvironment,
            ShellDescriptor shellDescriptor,
            ShellSettings shellSettings,
            MvcControllerManager defaultMvcControllerManager
            )
        {
            _typeFeatureProvider = typeFeatureProvider;
            _hostingEnvironment = hostingEnvironment;
            _shellSettings = shellSettings;
            _defaultMvcControllerManager = defaultMvcControllerManager;
        }

        public int Order
        {
            get
            {
                return -1000 + 10 - 1;
            }
        }

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            // This code is called only once per tenant during the construction of routes.
            // Or if an 'IActionDescriptorChangeProvider' tells that an action descriptor
            // has changed. E.g 'PageActionDescriptorChangeProvider' after any page update.


        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (var controller in context.Result.Controllers.Where(c => typeof(IAppService).IsAssignableFrom(c.ControllerType)))
            {
                var blueprint = _typeFeatureProvider.GetFeatureForDependency(controller.ControllerType);
                if (blueprint == null)
                    continue;
                if (!controller.RouteValues.ContainsKey("controller"))
                {
                    controller.RouteValues.Add("controller", controller.ControllerName.Replace("AppService", ""));
                }

                var areaName = blueprint.Name; //$"api/services/{blueprint.Name}";

                if (!controller.RouteValues.ContainsKey("area"))
                {
                    controller.RouteValues.Add("area", areaName);
                }
                else
                {
                    controller.RouteValues["area"] = areaName;
                }

                var controllerServiceInfo = _defaultMvcControllerManager.GetAll()
                    .FirstOrDefault(c => c.ServiceType == controller.ControllerType);
                var controllerServiceInterfaceAttrs = controllerServiceInfo.ServiceInterfaceType.GetCustomAttributes(false);
                
                var filterTypes = new Type[] { typeof(ApiControllerAttribute), typeof(IRouteTemplateProvider) };
                var defaultControllerAttrs =  new object[] { new ApiControllerAttribute(), new RouteAttribute("{area:exists}/{controller}/{action}") }; 
                var controllerAttrs = controller.Attributes as List<object>;
               
                var addAttrs = new List<object>();
                var isDefaultApiControllerAttribute = false;
                foreach (var filterType in filterTypes.Where(t => !controllerAttrs.Any(controllerAttr => t.IsAssignableFrom(controllerAttr.GetType()))))
                {
                    addAttrs.Add(controllerServiceInterfaceAttrs.Concat(defaultControllerAttrs).Where(a => filterType.IsAssignableFrom (a.GetType())).First());
                    if(filterType == typeof(ApiControllerAttribute))
                    {
                        controller.Filters.Add(addAttrs.Last() as IFilterMetadata);
                        isDefaultApiControllerAttribute = true;
                    }
                } 

                addAttrs.AddRange(controllerAttrs.Where(controllerAttr =>
                !filterTypes.Any(fType => fType.IsAssignableFrom(controllerAttr.GetType()))));
                //var action = controller.Actions.FirstOrDefault();
                if (addAttrs.Count > 0)
                {
                    controller.Selectors.Clear();
                    ConventionHelper.AddRange(controller.Selectors, ConventionHelper.CreateSelectors(addAttrs));
                }

                if (isDefaultApiControllerAttribute)
                    controllerAttrs.Add(new ApiControllerAttribute());


            }
        }
    }
}
