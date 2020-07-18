using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using BlocksCore.Data.Abstractions.Configurations;
using LinqToDB.Configuration;

namespace BlocksCore.Data.Linq2DB.SQLServer
{
    public class SQLServerDbContextOption : DbContextOption<LinqToDbConnectionOptions>
    {
        public SQLServerDbContextOption([NotNull] LinqToDbConnectionOptions option, [NotNull] IEnumerable<IDbContextOptionExtensions> extensions) : base(option, extensions)
        {
        }
    }
}
