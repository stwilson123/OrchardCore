using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BlocksCore.Autofac.Extensions.DependencyInjection;
using BlocksCore.Autofac.Extensions.DependencyInjection.Paramters;
using BlocksCore.Data.Linq2DB.Test.TestModel;
using BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext;
using BlocksCore.SyntacticAbstractions.Types.Collections;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlocksCore.Data.Linq2DB.Test.FunctionTest.TestModel
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
            testModelContexts = new List<TestModelContext>() {
               // new SqlserverModelContextFix(String.Format(ConfigurationHelper.Config[DatabaseConnectionStringConfigKey.SqlserverConnectString],dbName)),
                new OracleModelContextFix(String.Format(ConfigurationHelper.Config[DatabaseConnectionStringConfigKey.OracleConnectString],dbName),ConfigurationHelper.Config["BlocksEntities_OracleDBA"])

            };

            foreach (var modelContext in testModelContexts)
            {
                using (var dbContext = modelContext.ServiceProvider.GetService<MigrateDbContext>(new NamedParam("entityTypes", TestModelContext.registerTypes)))
                {
                    // dbContext.ExecuteSqlCommand("SELECT 1;");
                    dbContext.EnsureCreated();
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
                        //disconnect db
                       
                        // dbContext.ExecuteSqlCommand("SELECT 1;");
                         dbContext.EnsureDeleted();

                        modelContext.Dispose();
                    }
                    //using (var dbContext = new TestBlocksDbContext(new BlocksDbContextOption() { ProviderName = modelContext.ProviderName, ConnectString = modelContext.ConnectionString }))
                    //{
                    //    // dbContext.ExecuteSqlCommand("SELECT 1;");
                    //    dbContext.GetService<IRelationalDatabaseCreator>().EnsureDeleted();
                    //}

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
