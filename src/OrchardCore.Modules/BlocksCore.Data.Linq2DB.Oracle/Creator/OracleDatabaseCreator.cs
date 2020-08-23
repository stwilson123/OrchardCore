using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Migrator;
using BlocksCore.Data.Migrator;

namespace BlocksCore.Data.Linq2DB.Oracle.Creator
{
    public class OracleDatabaseCreator : DataBaseCreator
    {
        public OracleDatabaseCreator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
