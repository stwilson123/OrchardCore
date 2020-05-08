using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Data.Abstractions
{
    public interface IDataBaseProviderManager
    {
        IEnumerable<IDatabaseProvider> GetDatabaseProviders();

        IDatabaseProvider GetCurrentDatabaseProvider();
    }
}
