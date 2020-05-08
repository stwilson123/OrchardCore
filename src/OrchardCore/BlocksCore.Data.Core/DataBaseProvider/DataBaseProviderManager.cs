using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Data.Abstractions;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data
{
    public class DataBaseProviderManager : IDataBaseProviderManager
    {
        private readonly ShellSettings _shellSettings;
        private readonly IEnumerable<IDatabaseProvider> _databaseProviders;

        public DataBaseProviderManager(ShellSettings shellSettings, IEnumerable<IDatabaseProvider> databaseProviders)
        {
            _shellSettings = shellSettings;
            _databaseProviders = databaseProviders;
        }
        public IDatabaseProvider GetCurrentDatabaseProvider()
        {
            return _databaseProviders.FirstOrDefault(p => p.Name == _shellSettings["DatabaseProvider"]);
        }

        public IEnumerable<IDatabaseProvider> GetDatabaseProviders()
        {
            return _databaseProviders;
        }
    }
}
