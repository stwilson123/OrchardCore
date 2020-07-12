using System;
using BlocksCore.Data.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Modules;
using Microsoft.Data.SqlClient;
using LinqToDB.Data;

namespace BlocksCore.Data.Linq2DB.Sqlserver
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.TryAddDataProvider(new DatabaseProvider()
            {
                Name = "Sql Server",
                Value = "SqlConnection",
                HasConnectionString = true,
                SampleConnectionString = "Server=localhost;Database=Orchard;User Id=username;Password=password",
                HasTablePrefix = true,
                IsDefault = false,
                configBuilder = (optionBuilder, connection) =>
                {
                    var dbProvider = DataConnection.GetDataProvider("Microsoft.Data.SqlClient", connection.ConnectionString);
                    return optionBuilder.UseConnection(dbProvider, connection);
                },
                dbConnectionBuilder = (connectionString) => new SqlConnection(connectionString)
            });

            //if (EntityFrameworkServicesBuilder.CoreServices.TryGetValue(typeof(IMigrationsSqlGenerator), out ServiceCharacteristics serviceCharacteristics))
            //{
            //    //serviceCharacteristics.Lifetime.
            //    services.Replace(new ServiceDescriptor(typeof(IMigrationsSqlGenerator), typeof(SqlServerMigrationsSqlGeneratorEx), serviceCharacteristics.Lifetime));
            //}
            //TODO 

        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {

        }
    }
}
