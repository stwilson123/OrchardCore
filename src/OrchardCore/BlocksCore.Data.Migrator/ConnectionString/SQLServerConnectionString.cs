using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Infrastructure;

namespace BlocksCore.Data.Migrator.ConnectionString
{
    class SQLServerConnectionString : DynamicConnectionString
    {
        public SQLServerConnectionString(ConnectionInfo connectionInfo) : base(connectionInfo)
        {
        }

        public override string CurrentConnectionString
        {
            get
            {
                var databaseName = this.connectionInfo.Database;
                if (DbFunctionType == DbFunctionType.Bussness || string.IsNullOrEmpty(databaseName))
                    return this.connectionInfo.ConnectionString;

                return this.connectionInfo.ConnectionString.Replace(databaseName, "master");

            }
        }
    }
}
