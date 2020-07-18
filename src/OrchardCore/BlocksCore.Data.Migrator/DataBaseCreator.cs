using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Migrator;
using FluentMigrator.Runner;
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
                var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

                // Execute the migrations
                runner.MigrateUp();
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
