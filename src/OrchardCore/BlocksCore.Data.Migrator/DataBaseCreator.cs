using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BlocksCore.Autofac.Extensions.DependencyInjection;
using BlocksCore.Data.Abstractions.Infrastructure;
using BlocksCore.Data.Abstractions.Migrator;
using FluentMigrator.Infrastructure;
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
            using (var scope = serviceProvider.CreateScope())
            {
                var connectionStringAccessor = scope.ServiceProvider.GetRequiredService<DynamicConnectionString>();
                connectionStringAccessor.DbFunctionType = DbFunctionType.Master;
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>() as MigrationRunner;

                var migrationWrapper = new NonAttributedMigrationToMigrationInfoAdapter(runner.MigrationLoader.LoadMigrations()[20200730121800].Migration);

                // Execute the migrations
                runner.ApplyMigrationUp(migrationWrapper, false);

                connectionStringAccessor.DbFunctionType = DbFunctionType.Bussness;
            }
            Thread.Sleep(5 * 1000);
            return true;
        }


        public virtual bool EnsureDeleted()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var connectionStringAccessor = scope.ServiceProvider.GetRequiredService<DynamicConnectionString>();
                connectionStringAccessor.DbFunctionType = DbFunctionType.Master;
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>() as MigrationRunner;
               
                var migrationWrapper = new NonAttributedMigrationToMigrationInfoAdapter(runner.MigrationLoader.LoadMigrations()[20200730121800].Migration);
                // Execute the migrations
                runner.ApplyMigrationDown(migrationWrapper, false);

                connectionStringAccessor.DbFunctionType = DbFunctionType.Bussness;
            }
            return true;
        }
    }
}
