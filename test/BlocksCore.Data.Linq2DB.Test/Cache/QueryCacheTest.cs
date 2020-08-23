using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Data.Linq2DB.DBContext;
using BlocksCore.Data.Linq2DB.Test.FunctionTest.TestModel;
using BlocksCore.Data.Linq2DB.Test.TestConfiguration;
using BlocksCore.Data.Linq2DB.Test.TestConfiguration.Log;
using BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext;
using LinqToDB;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlocksCore.Data.Linq2DB.Test.Cache
{
    public class QueryCacheTest : IClassFixture<DbModelContextFixs>
    {
        private readonly DbModelContextFixs testModelContexts;

        public QueryCacheTest(DbModelContextFixs testModelContexts)
        {
            this.testModelContexts = testModelContexts;

            foreach (var testModelContext in this.testModelContexts.testModelContexts)
            {
                testModelContext.ServiceProvider.GetRequiredService<BlocksDbContext>().Insert(new TESTENTITY() { Id = testId, });
            } 
        }

        private string testId = Guid.NewGuid().ToString();

        [Theory]
        [MultDbData()]
        public void DoubleSingleOrDefault(string providerName)
        {
            var context = this.testModelContexts.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider.GetRequiredService<BlocksDbContext>();
            context.GetTable<TESTENTITY>().FirstOrDefault(t => t.Id == testId);
            var lastSql = context.LastQuery;
            context.LastQuery = "";
            //var commandLogCount = logHistory.Count();

            context.GetTable<TESTENTITY>().FirstOrDefault(t => t.Id == testId);
            Assert.True(lastSql == context.LastQuery, "Must generate sql command every times.");

        }



        //[Fact]
        //public void DoubleFind()
        //{
        //    foreach (var contextOption in contextOptions)
        //    {
        //        var logHistory = new List<string>();
        //        contextOption.loggerFactory = EFLoggerFactory.CreateLoggerFactory(logHistory);

        //        using (var context = new TestBlocksDbContext(contextOption))
        //        {
        //            context.TestEntity.Find(t => t.Id == testId);
        //            var commandLogCount = logHistory.Count();

        //            context.TestEntity.Find(testId);
        //            Assert.True(commandLogCount == logHistory.Count(), "Not more generate sql command because cached" );
        //        }


        //    }
        //}


    }
}
