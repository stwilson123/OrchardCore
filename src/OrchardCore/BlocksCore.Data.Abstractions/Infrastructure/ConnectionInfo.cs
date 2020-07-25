using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using BlocksCore.Data.Abstractions.DataBaseProvider;

namespace BlocksCore.Data.Abstractions.Infrastructure
{
    public class ConnectionInfo
    {
        public ConnectionInfo(string connectionString, DbConnectionStringBuilder dbConnectionStringBuilder, ProviderName providerName)
        {
            ConnectionString = connectionString;
            DBConnectionStringBuilder = dbConnectionStringBuilder;
            ProviderName = providerName;
        }

        public string ConnectionString { get; }

        public DbConnectionStringBuilder DBConnectionStringBuilder { get; }

        public ProviderName ProviderName { get; }

        public string Database {
            get {
                var result = string.Empty;
                switch(ProviderName)
                {
                    case ProviderName.SqlServer:
                    case ProviderName.SqlServer2000:
                    case ProviderName.SqlServer2005:
                    case ProviderName.SqlServer2008:
                    case ProviderName.SqlServer2012:
                    case ProviderName.SqlServer2014:
                    case ProviderName.SqlServer2017:
                        result = DBConnectionStringBuilder["Database"]?.ToString();break;
                    case ProviderName.Oracle:
                    case ProviderName.OracleManaged:
                    case ProviderName.OracleNative:
                        result = DBConnectionStringBuilder["USER ID"]?.ToString(); break;
                    default:
                        result = string.Empty; break;
                }
                return result;
            }
        }

    }
}
