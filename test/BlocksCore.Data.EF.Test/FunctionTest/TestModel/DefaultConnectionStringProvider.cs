using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Data.EF.Test.FunctionTest.TestModel
{
    public class DefaultConnectionStringProvider : BaseConnectionStinrgProvider
    {
        public override string getDbName()
        {
            return "TestBlockCoreDb";
        }
    }
}
