using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Data.EF.Test.FunctionTest.TestModel
{
    public class AutoGenernateConnectionStringProvider : BaseConnectionStinrgProvider
    {
        public override string getDbName()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
