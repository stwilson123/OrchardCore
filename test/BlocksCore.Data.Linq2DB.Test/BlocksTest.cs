//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using BlocksCore.Abstractions.DependencyInjection;
//using BlocksCore.Autofac.Extensions.DependencyInjection.Paramters;
//using BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext;
//using LinqToDB;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace BlocksCore.Data.Linq2DB.Test
//{
//    public class BlocksTest : IDisposable
//    {
//        protected IList<BlocksDbContextOption> contextOptions;

//        IServiceCollection services = new ServiceCollection();

//        public static IConfiguration InitConfiguration()
//        {
//            var config = new ConfigurationBuilder()
//                .AddJsonFile("appsettings.test.json")
//                .Build();
//            return config;
//        }

//        public BlocksTest()
//        {

//            services.AddTransient<BlocksDbContextOption>();
//            //services.AddDbContext<TestBlocksDbContext>(o => {
//            //    o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
//            //});
//            var serviceProvider = services.BuildServiceProvider();

//            var configuration = InitConfiguration();
//            contextOptions = new List<BlocksDbContextOption>()
//            {
//               new BlocksDbContextOption(){ ConnectString = string.Format(configuration[TestBlocksDbContext.SqlserverConnectString], Guid.NewGuid().ToString("N")),ProviderName = "SqlServer" },
//               new BlocksDbContextOption(){ ConnectString = string.Format(configuration[TestBlocksDbContext.OracleConnectString], Guid.NewGuid().ToString("N")),ProviderName = "SqlServer" }

//            };
//            foreach (var contextOption in contextOptions)
//            {
//                using (var context = new TestBlocksDbContext(contextOption))
//                {
//                    context.EnsureCreated();
//                }
//            }
//        }

//        public void Dispose()
//        {
//            var serviceProvider = services.BuildServiceProvider();
//            foreach (var contextOption in contextOptions)
//            {
//                using (var context = new TestBlocksDbContext(contextOption))
//                {
//                    context.EnsureDeleted();

//                }
//            }
//        }
//    }
//}
