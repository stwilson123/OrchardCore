using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions.Extensions;
using BlocksCore.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.EF.DBContext
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// </summary>
    public class BlocksDbContext : BaseBlocksDbContext,IDataContext
    {
        public BlocksDbContext(IEnumerable<Type> entityTypes, ShellSettings settingManager, ILogger<BlocksDbContext> log, DbContextOptions<BlocksDbContext> options) : base(entityTypes, settingManager, log, options)
        {
        }
    }
}
