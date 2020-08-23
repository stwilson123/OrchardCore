using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Infrastructure;

namespace BlocksCore.Data.Migrator.ConnectionString
{
    public class OracleConnectionString : DynamicConnectionString
    {
        private readonly string materConnectionString;

        public OracleConnectionString(ConnectionInfo connectionInfo,string materConnectionString) : base(connectionInfo)
        {
            this.materConnectionString = materConnectionString;
        }

        public override string CurrentConnectionString
        {
            get
            {
                if (DbFunctionType == DbFunctionType.Bussness )
                    return this.connectionInfo.ConnectionString;

                return materConnectionString;
            }
        }
    }
}
