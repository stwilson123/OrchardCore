using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Data.Linq2DB.Test.TestConfiguration;
using BlocksCore.Data.Linq2DB.Test.TestConfiguration.Log;
using BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext;
using LinqToDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;

namespace BlocksCore.Data.Linq2DB.Test.Cache
{
    public class QueryCacheTest : BlocksTest
    {

        private string testId = Guid.NewGuid().ToString();
        public QueryCacheTest() :base()
        {
            foreach (var contextOption in contextOptions)
            {
                using (var context = new TestBlocksDbContext(contextOption))
                {
                    context.Insert(new TESTENTITY() { Id = testId, });
                    
                }

            }
        }

        [Fact]
        public void DoubleSingleOrDefault()
        {
            foreach (var contextOption in contextOptions)
            {
                var logHistory = new List<string>();
                contextOption.loggerFactory = EFLoggerFactory.CreateLoggerFactory(logHistory);

                using (var context = new TestBlocksDbContext(contextOption))
                {
                    context.TestEntity.FirstOrDefault(t => t.Id == testId);
                    var commandLogCount = logHistory.Count();

                    context.TestEntity.FirstOrDefault(t => t.Id == testId);
                    Assert.True(commandLogCount < logHistory.Count(), "Must generate sql command every times.");
                }
            }

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
