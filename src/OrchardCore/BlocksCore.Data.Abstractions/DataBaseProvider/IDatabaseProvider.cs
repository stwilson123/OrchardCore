using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

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

        Func<string, DbConnection> DbConnectionBuilder { get; set; }
    }
}
