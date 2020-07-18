using System;
using System.Data.Common;
using BlocksCore.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BlocksCore.Data.EF
{
    public class DatabaseProvider : IDatabaseProvider
    {
        
        public string Name { get; set; }
        public string Value { get; set; }
        public bool HasConnectionString { get; set; }
        public bool HasTablePrefix { get; set; }
        public bool IsDefault { get; set; }
        public string SampleConnectionString { get; set; } = "";
        public Action<DbContextOptionsBuilder, DbConnection> configBuilder { get; set; }

        public Func<string,DbConnection> DbConnectionBuilder { get; set; }
    }
}
