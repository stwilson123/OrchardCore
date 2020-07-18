using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using BlocksCore.Data.Abstractions;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.Core
{
    public class DbConnectionAccessor : IDbConnectionAccessor
    {
        private readonly IDataBaseProviderManager _dataBaseProviderManager;
        private readonly ShellSettings _shellSettings;

        public DbConnectionAccessor(IDataBaseProviderManager dataBaseProviderManager, ShellSettings shellSettings)
        {
            if (dataBaseProviderManager?.GetDatabaseProviders() == null)
                throw new ArgumentNullException(nameof(IDatabaseProvider));
            _dataBaseProviderManager = dataBaseProviderManager;
            this._shellSettings = shellSettings;
        }

        public DbConnection CreateConnection()
        {
            var currentDBProvider = _dataBaseProviderManager.GetCurrentDatabaseProvider();
            if (currentDBProvider == null)
            {
                throw new BlocksDataException("CurrentDBProvider is null.");
            }

            var connectString = _shellSettings["ConnectionString"];

            return currentDBProvider.DbConnectionBuilder(connectString);
        }
    }
}
