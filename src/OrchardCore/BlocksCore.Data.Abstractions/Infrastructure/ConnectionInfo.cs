using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BlocksCore.Data.Abstractions.Infrastructure
{
    public class ConnectionInfo
    {
        public ConnectionInfo(string connectionString, string database)
        {
            ConnectionString = connectionString;
            Database = database;
        }

        public string ConnectionString { get; }

        public string Database { get; }



    }
}
