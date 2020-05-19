using System;
using System.Linq;
using BlocksCore.Navigation;
using BlocksCore.Navigation.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Extensions;
using OrchardCore.Modules;
 
using OrchardCore.Navigation;

namespace OrchardCore.Navigation
{
    public class Startup : StartupBase
    {
        private readonly IExtensionManager _extensionManager;

        public Startup(IExtensionManager extensionManager)
        {
            _extensionManager = extensionManager;
            //  this._shellDescriptor = shellDescriptor;
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddNavigation();
            services.AddNavigationCore();

            AutoRegisterTypes(services);
            //services.AddScoped<IShapeTableProvider, NavigationShapes>();
            //services.AddScoped<IShapeTableProvider, PagerShapesTableProvider>();
            //services.AddShapeAttributes<PagerShapes>();
        }
        
        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            
        }

        private void AutoRegisterTypes(IServiceCollection services)
        {
            foreach (var featureEntry in _extensionManager.LoadFeaturesAsync().Result)
            {
                var navigationFileProviders = featureEntry.ExportedTypes.Where(t => typeof(INavigationFileProvider).IsAssignableFrom(t));
                if (!navigationFileProviders.Any())
                    continue;

                foreach (var navigationFileProvider in navigationFileProviders)
                {
                    services.AddTransient(typeof(INavigationFileProvider), navigationFileProvider);
                }
            }
        }
    }
}
