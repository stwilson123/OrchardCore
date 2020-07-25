using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Data.Linq2DB.Test.TestModel
{
   public class DatabaseProviderName
    {
        public static readonly string Sqlserver = "Sql Server";
        public static readonly string Oracle = "Oracle";
        public static readonly string MySql = "MySql";

    }

    public class DatabaseConnectionStringConfigKey
    {
        public const string SqlserverConnectString = "BlocksEntities_Sqlserver";
        public const string OracleConnectString = "BlocksEntities_Oracle";
    }
}
