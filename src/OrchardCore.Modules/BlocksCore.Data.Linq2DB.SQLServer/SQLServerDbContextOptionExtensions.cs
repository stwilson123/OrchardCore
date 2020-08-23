using System;
using BlocksCore.Abstractions.Exception;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Abstractions.Infrastructure;
using BlocksCore.Data.Abstractions.Migrator;
using BlocksCore.Data.Linq2DB.SQLServer.Creator;
using BlocksCore.Data.Migrator;
using FluentMigrator.Runner;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BlocksCore.Data.Linq2DB.SQLServer
{
    public class SQLServerDbContextOptionExtensions : IDbContextOptionExtensions
    {

        public SQLServerDbContextOptionExtensions()
        {
        }

        public bool ApplyServices(IServiceCollection services,IServiceProvider serviceProvider, ConnectionInfo connectionInfo)
        {
            services.TryAddScoped<IDatabaseCreator, SQLServerDatabaseCreator>();
            //services.TryAddScoped<IModel>(sp => sp.GetService<IDbContextServices>().Model);
            //services.TryAddDataProvider((connectionString) =>
            //{
            //    return Linq2DBMap.GetDataProvider("Microsoft.Data.SqlClient", connectionString);
            //});
            services.AddMigratorSQLServer(connectionInfo,null);
            return true;
        }
    }
}
