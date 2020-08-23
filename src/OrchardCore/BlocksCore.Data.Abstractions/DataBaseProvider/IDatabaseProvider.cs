using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using BlocksCore.Data.Abstractions.DataBaseProvider;

namespace BlocksCore.Data.Abstractions
{
    public interface IDatabaseProvider
    {
       string Name { get;   }
       string Value { get; set; }
       bool HasConnectionString { get; set; }
       bool HasTablePrefix { get; set; }
       bool IsDefault { get; set; }
       string SampleConnectionString { get; set; }

        Func<string, DbConnection> CreateDbConnection { get; set; }

        Func<string, DbConnectionStringBuilder> GetConnectionStringBuilder { get; set; }

        Func<string, ProviderName> GetProviderName { get; set; }


    }
}
