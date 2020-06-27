using System;
using System.IO;
using System.Linq;
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
using BlocksCore.Domain.Abstractions.Domain;

namespace BlocksCore.Domain.Core
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
    

        }

        public override void ConfigureServices(IServiceCollection services)
        {

            AutoRegisterTypes(services);
        }
 

        private void AutoRegisterTypes(IServiceCollection services)
        {
            foreach (var featureEntry in _extensionManager.LoadFeaturesAsync().Result)
            {
                var domainServices = featureEntry.ExportedTypes.Where(t => typeof(IDomainService).IsAssignableFrom(t));
                if (!domainServices.Any())
                    continue;

                foreach (var domainService in domainServices)
                {
                    services.AddTransient(domainService);
                }
            }
        }



    }
}
