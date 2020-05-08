using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.EF.Test.TestModel.BlockTestContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.EF.Test
{
    public class BlocksTest : IDisposable
    {
        protected IList<BlocksDbContextOption> contextOptions;

        IServiceCollection services = new ServiceCollection();

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }

        public BlocksTest()
        {

            services.AddTransient<BlocksDbContextOption>();
            services.AddDbContext<TestBlocksDbContext>(o => {
                o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            var serviceProvider = services.BuildServiceProvider();
            var dtx = serviceProvider.GetService<TestBlocksDbContext>();


            var configuration = InitConfiguration();
            contextOptions = new List<BlocksDbContextOption>()
            {
               new BlocksDbContextOption(){ ConnectString = string.Format(configuration[TestBlocksDbContext.SqlserverConnectString], Guid.NewGuid().ToString("N")),ProviderName = "Sql Server" }
            };
            foreach (var contextOption in contextOptions)
            {
                using (var context = new TestBlocksDbContext(contextOption))
                {
                    context.GetService<IRelationalDatabaseCreator>().EnsureCreated();
                }
            }
        }

        public void Dispose()
        {
            foreach (var contextOption in contextOptions)
            {
                using (var context = new TestBlocksDbContext(contextOption))
                {
                    context.GetService<IRelationalDatabaseCreator>().EnsureDeleted();
                }
            }
        }
    }
}
