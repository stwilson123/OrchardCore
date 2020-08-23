using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Data.Abstractions;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Abstractions.Infrastructure;
using BlocksCore.Data.Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.Core.Configurations
{
    public class DbContextOptionExtensions : IDbContextOptionExtensions
    {

        public DbContextOptionExtensions()
        {
        }

        public bool ApplyServices(IServiceCollection services,IServiceProvider provider,ConnectionInfo connectionInfo)
        {

           
            services.TryAddSingleton<IDbContextServices, DbContextServices>();
            services.TryAddSingleton(provider.GetRequiredService<ShellSettings>());

            var dbProviderManager = provider.GetService<IDataBaseProviderManager>();
            services.TryAddSingleton(dbProviderManager.GetCurrentDatabaseProvider());
            services.TryAddSingleton(connectionInfo);
           
            return true;
        }
    }
}
