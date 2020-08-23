using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Infrastructure;

namespace BlocksCore.Data.Migrator
{
    public abstract class DynamicConnectionString
    {
        protected readonly ConnectionInfo connectionInfo;
        public DbFunctionType DbFunctionType { get; set; }

        public abstract string CurrentConnectionString { get; }
       


        public DynamicConnectionString(ConnectionInfo connectionInfo)
        {
            this.connectionInfo = connectionInfo;
        }
    }

    public enum DbFunctionType
    {
        Master,
        Bussness
    }
}
