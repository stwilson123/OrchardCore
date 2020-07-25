using System;
using BlocksCore.Data.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Modules;
using Microsoft.Data.SqlClient;
using LinqToDB.Data;
using LinqToDB.Configuration;
using BlocksCore.Data.Linq2DB.SQLServer;
using FluentMigrator;

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
                ConfigBuilder = (optionBuilder, connection) =>
                {
                    return new SQLServerDbContextOptionBuilder(optionBuilder, connection);
                },
                CreateDbConnection = (connectionString) => new SqlConnection(connectionString),
                GetConnectionStringBuilder = (connectionString) => new SqlConnectionStringBuilder(connectionString),
                GetProviderName = (connectionString) =>
                {
                    var dataProvider = Linq2DBMap.GetDataProvider("Microsoft.Data.SqlClient", connectionString);//TODO Opts serviceProvider.GetService<IDataProvider>();
                    return Linq2DBMap.Map(dataProvider.Name);
                }
            });


        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {

        }
    }

    public class TestMigrator : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {


        }
    }
}
