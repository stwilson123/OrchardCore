using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlocksCore.Data.EF.Test.TestModel.BlockTestContext
{

    public class TestBlocksDbContext : DbContext
    {
        private readonly bool autoDetectChangesEnabled = true;

        private readonly BlocksDbContextOption blocksDbContextOption;
        public const string SqlserverConnectString = "BlocksEntities_Sqlserver";
        public const string OracleConnectString = "BlocksEntities_Oracle";



        public TestBlocksDbContext(BlocksDbContextOption blocksDbContextOption)  
        {
            this.blocksDbContextOption = blocksDbContextOption;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (this.blocksDbContextOption.loggerFactory != null)
                optionsBuilder.UseLoggerFactory(this.blocksDbContextOption.loggerFactory);
            // var providerName = nameOrConnectionString.Split("_")[1];
            switch (this.blocksDbContextOption.ProviderName)
            {
                case "Sql Server": optionsBuilder.UseSqlServer(connectionString: this.blocksDbContextOption.ConnectString, sqlServerOptionsAction: option => option.UseRowNumberForPaging()); break;
                    //case "Oracle.ManagedDataAccess.Client":
                    //    optionsBuilder.UseOracle(connectionString, o => o.UseOracleSQLCompatibility("11")); break;
            }
            if (!autoDetectChangesEnabled)
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);


           // optionsBuilder.use
            //this.Database.Migrate();

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new TESTENTITYConfiguration());
            //modelBuilder.ApplyConfiguration(new TESTENTITY2Configuration());
            //modelBuilder.ApplyConfiguration(new TESTENTITY3Configuration());
        }

        public virtual DbSet<TESTENTITY> TestEntity { get; set; }
        public virtual DbSet<TESTENTITY2> TestEntity2 { get; set; }
        public virtual DbSet<TESTENTITY3> TestEntity3 { get; set; }
    }
    public class BlocksDbContextOption
    {
        public string ConnectString;
        public string ProviderName;
        public bool autoDetectChangesEnabled = false;
        public ILoggerFactory loggerFactory;
    }

}
