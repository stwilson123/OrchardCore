using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Core.Services;
using LinqToDB.Configuration;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext
{
    public class MigrateDbContext : BaseBlocksDbContext
    {
        protected override bool isDbMigrate { get; set; } = true;
        public MigrateDbContext(IEnumerable<Type> entityTypes, ShellSettings settingManager, ILogger<MigrateDbContext> log, DbContextOption<LinqToDbConnectionOptions>  options,ServiceProviderCache serviceProviderCache) : base(entityTypes,settingManager, log, options, serviceProviderCache)
        {
            ;
        }
    }
}
