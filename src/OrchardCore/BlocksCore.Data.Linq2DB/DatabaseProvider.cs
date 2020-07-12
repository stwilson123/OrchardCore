using System;
using System.Data.Common;
using BlocksCore.Data.Abstractions;
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
        public Func<LinqToDbConnectionOptionsBuilder, DbConnection, LinqToDbConnectionOptionsBuilder> configBuilder { get; set; }

        public Func<string,DbConnection> dbConnectionBuilder { get; set; }
    }
}
