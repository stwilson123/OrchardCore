using System;
using System.IO;
using System.Linq;
using BlocksCore.Application.Abstratctions;
using BlocksCore.WebAPI.Controllers;
using BlocksCore.WebAPI.Controllers.Builder;
using BlocksCore.WebAPI.Controllers.Factory;
using BlocksCore.WebAPI.Controllers.Manager;
using BlocksCore.WebAPI.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Descriptor.Models;
using OrchardCore.Modules;
using Microsoft.AspNetCore.Mvc;
using BlocksCore.Web.Abstractions.Filters;

namespace BlocksCore.WebAPI.Providers
{
    public class Startup : StartupBase
    {
        public override int Order => -1000;
        public override int ConfigureOrder => 1000;

        private readonly IServiceProvider _serviceProvider;

        private readonly IExtensionManager _extensionManager;
        // private readonly ShellDescriptor _shellDescriptor;

        public Startup(IServiceProvider serviceProvider, IExtensionManager extensionManager/*, ShellDescriptor shellDescriptor*/)
        {
            _serviceProvider = serviceProvider;
            _extensionManager = extensionManager;
            //  this._shellDescriptor = shellDescriptor;
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            //TODO area:exists not work area "api/aa"
            routes.MapControllerRoute("BlocksDefault", "{area}/{controller}/{action}/{id?}");

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            MvcControllerManager defaultMvcControllerManager = new MvcControllerManager();
            services.AddSingleton(defaultMvcControllerManager);
            var builder = services.AddMvc(options =>
            {
                //options.Conventions.Add(new ControllerModelConvention(defaultMvcControllerManager));
                options.Conventions.Add(new ActionModelConvention(defaultMvcControllerManager));
                options.Filters.Add(new DefaultActionFilter());
                options.Filters.Add(new DefaultResultFilter());
                options.Filters.Add(new DefaultExceptionFilter());


                var autoValidate = options.Filters.FirstOrDefault(f => f is TypeFilterAttribute typeFilter && typeFilter.ImplementationType == typeof(AutoValidateAntiforgeryTokenAttribute));
                if (autoValidate != null)
                    options.Filters.Remove(autoValidate);
            });
           
            AddModularFrameworkParts(_serviceProvider, builder.PartManager, defaultMvcControllerManager);
            AddMvcModuleCoreServices(services);
        }

        internal void AddModularFrameworkParts(IServiceProvider services, ApplicationPartManager manager, MvcControllerManager defaultMvcControllerManager)
        {

            // var features = _shellDescriptor.Features;

            new ApiControllerConventional(_extensionManager.LoadFeaturesAsync().Result,
                new MvcControllerBuilderFactory(new MvcControllerOption(typeof(IAppService)), defaultMvcControllerManager)).RegisterController();
            //manager.ApplicationParts.Insert(0, new ShellFeatureApplicationPart());
            manager.FeatureProviders.Add(new ServiceControllerFeatureProvider(defaultMvcControllerManager));
        }

        internal void AddMvcModuleCoreServices(IServiceCollection services)
        {

            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IApplicationModelProvider, ModularApplicationModelProvider>());
            //services.TryAddEnumerable(
            //    ServiceDescriptor.Singleton<IActionDescriptorProvider, ModularActionDescriptorProvider>());

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

    }
}
