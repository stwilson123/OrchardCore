using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Data.Linq2DB.Test.FunctionTest.TestModel
{
    public class AutoGenernateConnectionStringProvider : BaseConnectionStinrgProvider
    {
        public override string getDbName()
        {
            return "a"+ Guid.NewGuid().ToString("N").Substring(0,29);
        }
    }
}
