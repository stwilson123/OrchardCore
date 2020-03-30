using System.Collections.Generic;
using System.Linq;
using BlocksCore.Application.Abstratctions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
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

        public ModularApplicationModelProvider(
            ITypeFeatureProvider typeFeatureProvider,
            IHostEnvironment hostingEnvironment,
            ShellDescriptor shellDescriptor,
            ShellSettings shellSettings)
        {
            _typeFeatureProvider = typeFeatureProvider;
            _hostingEnvironment = hostingEnvironment;
            _shellSettings = shellSettings;
        }

        public int Order
        {
            get
            {
                return 1000 - 1;
            }
        }

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            // This code is called only once per tenant during the construction of routes.
            // Or if an 'IActionDescriptorChangeProvider' tells that an action descriptor
            // has changed. E.g 'PageActionDescriptorChangeProvider' after any page update.

            foreach (var controller in context.Result.Controllers.Where(c => typeof(IAppService).IsAssignableFrom(c.ControllerType)))
            {
                var blueprint = _typeFeatureProvider.GetFeatureForDependency(controller.ControllerType);
                if (blueprint == null)
                    continue;
                if (!controller.RouteValues.ContainsKey("controller"))
                {
                    controller.RouteValues.Add("controller", controller.ControllerName.Replace("AppService", ""));
                }

                var areaName = $"api/services/{blueprint.Name}";

                if (!controller.RouteValues.ContainsKey("area"))
                {
                    controller.RouteValues.Add("area", areaName);
                }
                else
                {
                    controller.RouteValues["area"] =  areaName;
                }
                var action = controller.Actions.FirstOrDefault();

              //  var actions = action.Attributes as IList<object>;
              //  actions.Add(new HttpGetAttribute());
                //var blueprint = _typeFeatureProvider.GetFeatureForDependency(controllerType);

                //if (blueprint != null)
                //{
                //    if (blueprint.Extension.Id == _hostingEnvironment.ApplicationName &&
                //        _shellSettings.State != TenantState.Running)
                //    {
                //        // Don't serve any action of the application'module which is enabled during a setup.IApplicationFeatureProvider
                //        foreach (var action in controller.Actions)
                //        {
                //            action.Selectors.Clear();
                //        }

                //        controller.Selectors.Clear();
                //    }
                //    else
                //    {
                //        // Add an "area" route value equal to the module id.
                //        controller.RouteValues.Add("area", blueprint.Extension.Id);
                //    }
                //}
            }
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
        }
    }
}
