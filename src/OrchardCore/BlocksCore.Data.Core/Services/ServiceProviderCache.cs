using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using BlocksCore.Autofac.Extensions.DependencyInjection;
using BlocksCore.Data.Abstractions;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Abstractions.Infrastructure;
using BlocksCore.SyntacticAbstractions.Collection;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;

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
            return SerivceProviderFactory.CreateServiceProvider(null, services, Enumerable.Empty<ServiceDescriptor>());
        }

        private static bool ApplyServices(IDbContextOptions options, IServiceCollection services,IServiceProvider serviceProvider)
        {
            var coreServicesAdded = false;
            var connectionString = serviceProvider.GetRequiredService<ShellSettings>()["ConnectionString"];
            var dataProvider = serviceProvider.GetRequiredService<IDatabaseProvider>();
            var connectionStringBuilder = dataProvider.GetConnectionStringBuilder(connectionString);
            var providerName = dataProvider.GetProviderName(connectionString);
           
            var connectInfo = new ConnectionInfo(connectionString, connectionStringBuilder, providerName);
            foreach (var extension in options.Extensions)
            {
                if (extension.ApplyServices(services, serviceProvider, connectInfo))
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
