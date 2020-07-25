using System;
using System.Data.Common;
using BlocksCore.Data.Abstractions;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Abstractions.DataBaseProvider;
using BlocksCore.Domain.Abstractions;
using LinqToDB.Configuration;

namespace BlocksCore.Data.Linq2DB
{
    public class DatabaseProvider : IDatabaseProvider
    {
        
        public string Name { get; set; }
        public string Value { get; set; }
        public bool HasConnectionString { get; set; }
        public bool HasTablePrefix { get; set; }
        public bool IsDefault { get; set; }
        public string SampleConnectionString { get; set; } = "";
        public Func<IDbContextOptionBuilder<LinqToDbConnectionOptions>, IUnitOfWork, IDbContextOptionBuilder< LinqToDbConnectionOptions>> ConfigBuilder { get; set; }

        public Func<string,DbConnection> CreateDbConnection { get; set; }
        public Func<string, DbConnectionStringBuilder> GetConnectionStringBuilder { get ; set ; }
        public Func<string, ProviderName> GetProviderName { get; set; }
    }
}
