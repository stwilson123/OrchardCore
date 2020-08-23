using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BlocksCore.Data.Abstractions.Configurations
{
    public class DbContextOption<TDbOption> : IDbContextOptions
    {
        public TDbOption Option { get; }

        public IEnumerable<IDbContextOptionExtensions> Extensions { get; set; }

        public DbContextOption([NotNull] TDbOption option, [NotNull] IEnumerable<IDbContextOptionExtensions> extensions)
        {
            Option = option;
            Extensions = extensions;
        }
    }
}
