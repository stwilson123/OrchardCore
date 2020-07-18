using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions.Exception;
using BlocksCore.Data.Abstractions.DataBaseProvider;
using BlocksCore.SyntacticAbstractions.Collection;
using LinqToDB.Data;
using LinqToDB.DataProvider;

namespace BlocksCore.Data.Linq2DB
{
    public static class Linq2DBMap
    {
        static Dictionary<string, ProviderName> ProviderNames = new Dictionary<string, ProviderName>() {
            { LinqToDB.ProviderName.SqlServer2005,ProviderName.SqlServer2005 },
            { LinqToDB.ProviderName.SqlServer2008,ProviderName.SqlServer2008 },
            { LinqToDB.ProviderName.SqlServer2012,ProviderName.SqlServer2012 },
            { LinqToDB.ProviderName.SqlServer2014,ProviderName.SqlServer2014 },
            { LinqToDB.ProviderName.SqlServer2017,ProviderName.SqlServer2017 },
            { LinqToDB.ProviderName.OracleManaged,ProviderName.OracleManaged },
            { LinqToDB.ProviderName.MySqlOfficial,ProviderName.MySqlOfficial },

        };

        static LazyConcurrentDictionary<(string,string), IDataProvider> connectMapDataProviderCache = new LazyConcurrentDictionary<(string, string), IDataProvider>();
        public static ProviderName Map(string providerName)
        {
            if (ProviderNames.TryGetValue(providerName, out ProviderName provider))
                return provider;

            throw new BlocksException("", $"ProviderName {providerName} is not mapping.");
        }

        public static IDataProvider GetDataProvider(string providerName,string connectionString)
        {
            return connectMapDataProviderCache.GetOrAdd((providerName, connectionString), (data) =>
            {
                return DataConnection.GetDataProvider(data.Item1, data.Item2);
            });
        }

    }
}
