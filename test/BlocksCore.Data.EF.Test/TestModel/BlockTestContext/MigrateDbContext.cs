using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.EF.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.EF.Test.TestModel.BlockTestContext
{
    public class MigrateDbContext : BaseBlocksDbContext
    {

        protected override bool isDbMigrate => true;
        public MigrateDbContext(IEnumerable<Type> entityTypes, ShellSettings settingManager, ILogger<MigrateDbContext> log, DbContextOptions<MigrateDbContext> options) : base(entityTypes, settingManager, log, options)
        {

        }
    }
}
