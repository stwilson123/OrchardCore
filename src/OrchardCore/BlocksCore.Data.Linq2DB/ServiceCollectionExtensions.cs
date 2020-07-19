using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BlocksCore.Data.Abstractions.Infrastructure;
using LinqToDB.DataProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.Linq2DB
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection TryAddDataProvider(this IServiceCollection services,IServiceProvider serviceProvider, Func<string, IDataProvider> optionbuilder)
        {

            if(optionbuilder != null)
            {
                var shellSettings = serviceProvider.GetService<ShellSettings>();
                var connectionString = shellSettings["ConnectionString"];
                var dataProvider = optionbuilder(connectionString);
                using (var connection = dataProvider.CreateConnection(connectionString))
                {
                    services.TryAddScoped<IDataProvider>(sp => dataProvider);
                    var info = new ConnectionInfo(connectionString, connection.Database);
                    services.TryAddScoped<ConnectionInfo>(sp => info);
                    services.TryAddScoped<Func<string, IDbConnection>>((sp) => (connectionString) => dataProvider.CreateConnection(connectionString));
                }
            }
            return services;
        }

    }
}
