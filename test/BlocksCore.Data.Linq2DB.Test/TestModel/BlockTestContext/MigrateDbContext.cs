using System;
using System.Collections.Generic;
using System.Text;
using LinqToDB.Configuration;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext
{
    public class MigrateDbContext : BaseBlocksDbContext
    {
        protected override bool isDbMigrate { get; set; } = true;
        public MigrateDbContext(IEnumerable<Type> entityTypes, ShellSettings settingManager, ILogger<MigrateDbContext> log, LinqToDbConnectionOptions options) : base(entityTypes,settingManager, log, options)
        {
            ;
        }
    }
}
