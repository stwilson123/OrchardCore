using System;
using BlocksCore.Data.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
using OrchardCore.Modules;

namespace BlocksCore.Data.EF.Oracle
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
                    optionBuilder.UseOracle(connection);
                },
                dbConnectionBuilder = (connectionString) => new OracleConnection(connectionString)
            });
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {

        }
    }
}
