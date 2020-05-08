using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Autofac.Extensions.DependencyInjection;
using BlocksCore.Autofac.Extensions.DependencyInjection.Paramters;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Test.TestModel.BlockTestContext;
using BlocksCore.SyntacticAbstractions.Types.Collections;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
namespace BlocksCore.Data.EF.Test.FunctionTest.TestModel
{
    public class DbModelContextFixs : IDisposable
    {
        public IEnumerable<TestModelContext> testModelContexts { get; } 
        public DbModelContextFixs()
        {
            testModelContexts = new List<TestModelContext>() {
                new SqlserverModelContextFix()
            };


            foreach (var modelContext in testModelContexts)
            {
                using (var dbContext = modelContext.ServiceProvider.GetService<MigrateDbContext>(new NamedParam("entityTypes", TestModelContext.registerTypes)))
                {
                   // dbContext.ExecuteSqlCommand("SELECT 1;");
                    dbContext.GetService<IRelationalDatabaseCreator>().EnsureCreated();
                }
            }
        }
        public void Dispose()
        {
            var listExceptions = new List<Exception>();
            foreach (var modelContext in testModelContexts)
            {
                try
                {

                    using (var dbContext = modelContext.ServiceProvider.GetService<MigrateDbContext>(new NamedParam("entityTypes", TestModelContext.registerTypes)))
                    {
                        // dbContext.ExecuteSqlCommand("SELECT 1;");
                        dbContext.GetService<IRelationalDatabaseCreator>().EnsureDeleted();
                    }
                    //using (var dbContext = new TestBlocksDbContext(new BlocksDbContextOption() { ProviderName = modelContext.ProviderName, ConnectString = modelContext.ConnectionString }))
                    //{
                    //    // dbContext.ExecuteSqlCommand("SELECT 1;");
                    //    dbContext.GetService<IRelationalDatabaseCreator>().EnsureDeleted();
                    //}
                    modelContext.Dispose();
                }
                catch(Exception ex)
                {
                    listExceptions.Add(ex);
                }
                finally
                {

                }
            }
            if(!listExceptions.IsNullOrEmpty())
                 throw new Exception(string.Join("\r\n",listExceptions.Select(e => e.Message)));
        }
    }
}
