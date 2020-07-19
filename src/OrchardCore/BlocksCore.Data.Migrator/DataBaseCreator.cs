using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Autofac.Extensions.DependencyInjection;
using BlocksCore.Data.Abstractions.Infrastructure;
using BlocksCore.Data.Abstractions.Migrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.Migrator
{
    public abstract class DataBaseCreator : IDatabaseCreator
    {
        private readonly IServiceProvider serviceProvider;

        public DataBaseCreator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public virtual bool EnsureCreated()
        {


           // SerivceProviderFactory.CreateServiceProvider(serviceProvider, null, null);
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                var connectionStringAccessor = scope.ServiceProvider.GetRequiredService<DynamicConnectionString>();
                var databaseName = scope.ServiceProvider.GetRequiredService<IDbContextServices>().GetConnetionInfo().Database;
                connectionStringAccessor.ConnectionString = connectionStringAccessor.ConnectionString.Replace(databaseName, "master");
                

                // Execute the migrations
                runner.MigrateUp(20200730121800);
            }
            return true;
        }
        

        public virtual bool EnsureDeleted()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
                // Execute the migrations
                runner.MigrateDown(20200730121800);
            }
            return true;
        }
    }
}
