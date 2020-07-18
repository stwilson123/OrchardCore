using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Data.Abstractions.Migrator
{
    public interface IDatabaseCreator  
    {
        bool EnsureCreated();

        bool EnsureDeleted();
    }
}
