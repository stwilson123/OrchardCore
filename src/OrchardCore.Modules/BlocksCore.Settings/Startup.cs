using System;
using System.Linq;
using BlocksCore.Settings.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Extensions;
using OrchardCore.Modules;
using OrchardCore.Settings;

namespace BlocksCore.Settings
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
            services.AddScoped<ISiteService, SiteService>();
        }
        
        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            
        }

    }
}
