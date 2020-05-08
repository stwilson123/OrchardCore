using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Data.EF.Test.TestConfiguration;
using BlocksCore.Data.EF.Test.TestConfiguration.Log;
using BlocksCore.Data.EF.Test.TestModel.BlockTestContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;

namespace BlocksCore.Data.EF.Test.Cache
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
                    context.TestEntity.Add(new TESTENTITY() { Id = testId, });
                    context.SaveChanges();
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



        [Fact]
        public void DoubleFind()
        {
            foreach (var contextOption in contextOptions)
            {
                var logHistory = new List<string>();
                contextOption.loggerFactory = EFLoggerFactory.CreateLoggerFactory(logHistory);

                using (var context = new TestBlocksDbContext(contextOption))
                {
                    context.TestEntity.Find(testId);
                    var commandLogCount = logHistory.Count();

                    context.TestEntity.Find(testId);
                    Assert.True(commandLogCount == logHistory.Count(), "Not more generate sql command because cached" );
                }


            }
        }


    }
}
