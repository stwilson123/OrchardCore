using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Abstractions.Extensions;
using BlocksCore.Domain.Abstractions;
using LinqToDB.Configuration;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.Linq2DB.DBContext
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// </summary>
    public class BlocksDbContext : BaseBlocksDbContext,IDataContext
    {
        public BlocksDbContext(IEnumerable<Type> entityTypes,ShellSettings settingManager, ILogger<BlocksDbContext> log, LinqToDbConnectionOptions options) : base(entityTypes, settingManager, log, options)
        {
        }

        
    }
}
