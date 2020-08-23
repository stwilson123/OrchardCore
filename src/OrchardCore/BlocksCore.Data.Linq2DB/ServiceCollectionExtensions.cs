//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Text;
//using BlocksCore.Data.Abstractions.Infrastructure;
//using LinqToDB.DataProvider;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection.Extensions;
//using OrchardCore.Environment.Shell;

//namespace BlocksCore.Data.Linq2DB
//{
//    public static class ServiceCollectionExtensions
//    {
//        public static IServiceCollection TryAddDataProvider(this IServiceCollection services, Func<string, IDataProvider> optionbuilder)
//        {
            
//            if (optionbuilder != null)
//            {
//                services.TryAddScoped<IDataProvider>(sp => optionbuilder(sp.GetRequiredService<ConnectionInfo>().ConnectionString));
//                services.TryAddScoped<Func<string, IDbConnection>>((sp) => (connectionString) => sp.GetRequiredService<IDataProvider>().CreateConnection(connectionString));

//            }
//            return services;
//        }

//    }
//}
