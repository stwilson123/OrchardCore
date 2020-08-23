using System;
using BlocksCore.Abstractions.Exception;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Abstractions.Infrastructure;
using BlocksCore.Data.Abstractions.Migrator;
using BlocksCore.Data.Linq2DB.Oracle.Creator;
using BlocksCore.Data.Migrator;
using BlocksCore.Data.Migrator.ConnectionString;
using FluentMigrator.Runner;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.Linq2DB.Oracle
{
    public class OracleDbContextOptionExtensions : IDbContextOptionExtensions
    {

        public OracleDbContextOptionExtensions()
        {
        }

        public bool ApplyServices(IServiceCollection services,IServiceProvider serviceProvider, ConnectionInfo connectionInfo)
        {
            services.TryAddScoped<IDatabaseCreator, OracleDatabaseCreator>();
            //services.TryAddScoped<IModel>(sp => sp.GetService<IDbContextServices>().Model);
            //services.TryAddDataProvider((connectionString) =>
            //{
            //    return Linq2DBMap.GetDataProvider("Microsoft.Data.SqlClient", connectionString);
            //});
            services.AddMigratorCore(connectionInfo, null);
            var shellSettings = serviceProvider.GetRequiredService<ShellSettings>();
            services.AddScoped<DynamicConnectionString>(sp => new OracleConnectionString(sp.GetService<ConnectionInfo>(), shellSettings["MasterConnectionString"]));
            return true;
        }
    }
}
