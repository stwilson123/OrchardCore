using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext
{

    public class TestBlocksDbContext : BaseBlocksDbContext
    {
        private readonly bool autoDetectChangesEnabled = true;

        private readonly BlocksDbContextOption blocksDbContextOption;
        public const string SqlserverConnectString = "BlocksEntities_Sqlserver";
        public const string OracleConnectString = "BlocksEntities_Oracle";


        public TestBlocksDbContext(BlocksDbContextOption blocksDbContextOption) : base(null, null, blocksDbContextOption.ConnectString)
        {
            this.blocksDbContextOption = blocksDbContextOption;
        }
        public TestBlocksDbContext(ShellSettings settingManager,BlocksDbContextOption blocksDbContextOption,ILogger logger) :base(settingManager, logger, blocksDbContextOption.ConnectString) 
        {
            this.blocksDbContextOption = blocksDbContextOption;
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (this.blocksDbContextOption.loggerFactory != null)
        //        optionsBuilder.UseLoggerFactory(this.blocksDbContextOption.loggerFactory);
        //    // var providerName = nameOrConnectionString.Split("_")[1];
        //    switch (this.blocksDbContextOption.ProviderName)
        //    {
        //        case "Sql Server": optionsBuilder.UseSqlServer(connectionString: this.blocksDbContextOption.ConnectString, sqlServerOptionsAction: option => option.UseRowNumberForPaging()); break;
        //            //case "Oracle.ManagedDataAccess.Client":
        //            //    optionsBuilder.UseOracle(connectionString, o => o.UseOracleSQLCompatibility("11")); break;
        //    }
        //    if (!autoDetectChangesEnabled)
        //        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        //}
      

        public virtual ITable<TESTENTITY> TestEntity { get => this.GetTable<TESTENTITY>(); }
        public virtual ITable<TESTENTITY2> TestEntity2 { get => this.GetTable<TESTENTITY2>(); }
        public virtual ITable<TESTENTITY3> TestEntity3 { get => this.GetTable<TESTENTITY3>(); }
    }
    public class BlocksDbContextOption
    {
        public string ConnectString;
        public string ProviderName;
        public bool autoDetectChangesEnabled = false;
        public ILoggerFactory loggerFactory;
    }

}
