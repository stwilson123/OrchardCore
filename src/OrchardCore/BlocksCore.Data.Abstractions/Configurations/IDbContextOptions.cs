using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Data.Abstractions.Configurations
{
    public interface IDbContextOptions
    {

        public IEnumerable<IDbContextOptionExtensions> Extensions { get; set; }
    }
}
