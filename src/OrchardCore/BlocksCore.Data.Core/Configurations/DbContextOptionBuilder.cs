using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Configurations;

namespace BlocksCore.Data.Core.Configurations
{
    public class DbContextOptionBuilder<TDbContextOption> : IDbContextOptionBuilder<TDbContextOption>
    {

        public IList<IDbContextOptionExtensions> Extensions { get; private set; } = new List<IDbContextOptionExtensions>();

        TDbContextOption _dbContextOption;


        public DbContextOptionBuilder()
        {
            AddOrUpdateExtension(new DbContextOptionExtensions());
        }


        public IDbContextOptionBuilder<TDbContextOption> AddOrUpdateExtension(IDbContextOptionExtensions extensions)
        {
            Extensions.Add(extensions);
            return this;
        }

        public IDbContextOptionBuilder<TDbContextOption> WithOption(TDbContextOption option)
        {
            _dbContextOption = option;
            return this;

        }

        public virtual DbContextOption<TDbContextOption> Build()
        {
            return new DbContextOption<TDbContextOption>(_dbContextOption, Extensions);
        }
    }
}
