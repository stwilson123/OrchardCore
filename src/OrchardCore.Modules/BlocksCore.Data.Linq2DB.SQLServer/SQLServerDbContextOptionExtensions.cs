using System;
using BlocksCore.Abstractions.Exception;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Abstractions.Infrastructure;
using BlocksCore.Data.Abstractions.Migrator;
using BlocksCore.Data.Linq2DB.SQLServer.Creator;
using BlocksCore.Data.Migrator;
using FluentMigrator.Runner;
using LinqToDB.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BlocksCore.Data.Linq2DB.SQLServer
{
    public class SQLServerDbContextOptionExtensions : IDbContextOptionExtensions
    {
        public bool ApplyServices(IServiceCollection services,IServiceProvider serviceProvider)
        {
            services.TryAddScoped<IDatabaseCreator, SQLServerDatabaseCreator>();
            services.TryAddScoped<IModel>(sp => sp.GetService<IDbContextServices>().Model);
            services.AddMigratorCore(serviceProvider,(builder,connectionString) => {

                var dbProvider = Linq2DBMap.GetDataProvider("Microsoft.Data.SqlClient", connectionString);
                if (dbProvider == null)
                    throw new BlocksException("","Not found Linq2Db DataProvider.");

                builder.AddDataBaseProvider(Linq2DBMap.Map(dbProvider.Name));
            });
            return true;
        }
    }
}
