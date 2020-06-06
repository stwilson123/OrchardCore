using System;
using BlocksCore.Data.Abstractions;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Data.Core.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataCore(this IServiceCollection services)
        {
            services.AddTransient<IDbConnectionAccessor, DbConnectionAccessor>();
            services.AddSingleton<IDataBaseProviderManager, DataBaseProviderManager>();
            services.AddScoped<IUnitOfWorkManager, DefaultUnitOfWorkManager>();
            services.AddScoped<ICurrentUnitOfWorkProvider, DefaultCurrentUnitOfWorkProvider>();


            return services;
        }

        public static IServiceCollection TryAddDataProvider(this IServiceCollection services, IDatabaseProvider databaseProvider)
        {
            for (var i = services.Count - 1; i >= 0; i--)
            {
                var entry = services[i];
                if (entry.ImplementationInstance != null)
                {
                    var existDatabaseProvider = entry.ImplementationInstance as IDatabaseProvider;
                    if (databaseProvider != null && existDatabaseProvider != null && String.Equals(existDatabaseProvider.Name, databaseProvider.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        services.RemoveAt(i);
                    }
                }
            }

            services.AddSingleton(databaseProvider);

            return services;
        }


    }
}
