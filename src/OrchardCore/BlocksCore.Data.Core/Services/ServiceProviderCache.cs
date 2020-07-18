using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.SyntacticAbstractions.Collection;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.Core.Services
{
    public class ServiceProviderCache
    {
        private readonly LazyConcurrentDictionary<IDbContextOptions, IServiceProvider> _configurations
           = new LazyConcurrentDictionary<IDbContextOptions, IServiceProvider>();
        private readonly IServiceProvider serviceProvider;

        public ServiceProviderCache(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public virtual IServiceProvider GetOrAdd([NotNull] IDbContextOptions options)
        {
            var serviceProvider = _configurations.GetOrAdd(options, (t) =>
             {
                 return BuildServiceProvider(t);
             });
            return serviceProvider;
        }

        private IServiceProvider BuildServiceProvider(IDbContextOptions options)
        {
            var services = new ServiceCollection();
            var hasProvider = ApplyServices(options, services, serviceProvider);
            return services.BuildServiceProvider();

        }

        private static bool ApplyServices(IDbContextOptions options, IServiceCollection services,IServiceProvider serviceProvider)
        {
            var coreServicesAdded = false;

            foreach (var extension in options.Extensions)
            {
                if (extension.ApplyServices(services, serviceProvider))
                {
                    coreServicesAdded = true;
                }
            }

            if (coreServicesAdded)
            {
                return true;
            }

            //  new EntityFrameworkServicesBuilder(services).TryAddCoreServices();

            return false;
        }
    }
}
