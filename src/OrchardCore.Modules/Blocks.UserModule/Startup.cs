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
using OrchardCore.Security;

namespace Blocks.UserModule
{
    public class Startup : StartupBase
    {

        public Startup()
        {
            
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSecurity();

        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            
        }

        
    }
}
