using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Abstractions.Infrastructure;
using BlocksCore.Data.Core.Infrastructure;
using LinqToDB.DataProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.Linq2DB.Configurations
{
    public class Linq2DbDbContextOptionExtensions : IDbContextOptionExtensions
    {

        public Linq2DbDbContextOptionExtensions()
        {
        }

        public bool ApplyServices(IServiceCollection services,IServiceProvider provider)
        {
            //services.TryAddScoped<ConnectionInfo>(sp => {
                
            //     var dataProvider = provider.GetService<IDataProvider>()
            //    return new ConnectionInfo(dataProvider.),
            //    });
           

            return true;
        }
    }
}
