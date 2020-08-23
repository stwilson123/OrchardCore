using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Migrator;
using BlocksCore.Data.Migrator;

namespace BlocksCore.Data.Linq2DB.SQLServer.Creator
{
    public class SQLServerDatabaseCreator : DataBaseCreator
    {
        public SQLServerDatabaseCreator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
