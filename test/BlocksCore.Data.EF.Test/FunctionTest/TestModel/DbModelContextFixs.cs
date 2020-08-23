using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Autofac.Extensions.DependencyInjection;
using BlocksCore.Autofac.Extensions.DependencyInjection.Paramters;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Test.TestModel;
using BlocksCore.Data.EF.Test.TestModel.BlockTestContext;
using BlocksCore.SyntacticAbstractions.Types.Collections;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlocksCore.Data.EF.Test.FunctionTest.TestModel
{
    public class DbModelContextFixs : BaseDbModelContextFixs<AutoGenernateConnectionStringProvider>
    {
        public DbModelContextFixs() : base()
        {
        }
    }

    public class SpecialDbModelContextFixs : BaseDbModelContextFixs<DefaultConnectionStringProvider>
    {
        public SpecialDbModelContextFixs() : base()
        {
        }
    }


    public class BaseDbModelContextFixs<T> : IDisposable, IClassFixture<T> where T : BaseConnectionStinrgProvider,new()
    {
        public IEnumerable<TestModelContext> testModelContexts { get; }
        public BaseDbModelContextFixs()
        {
            T connectionStinrgProvider = new T();
            var dbName = connectionStinrgProvider.getDbName();
            var autoConnectString = String.Format(ConfigurationHelper.Config[TestBlocksDbContext.SqlserverConnectString],dbName);
            testModelContexts = new List<TestModelContext>() {
                new SqlserverModelContextFix(autoConnectString)
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
                catch (Exception ex)
                {
                    listExceptions.Add(ex);
                }
                finally
                {

                }
            }
            if (!listExceptions.IsNullOrEmpty())
                throw new Exception(string.Join("\r\n", listExceptions.Select(e => e.Message)));
        }
    }
}
