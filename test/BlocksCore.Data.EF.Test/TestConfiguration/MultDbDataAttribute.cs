using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BlocksCore.Data.EF.Test.TestModel;
using Xunit.Sdk;

namespace BlocksCore.Data.EF.Test.TestConfiguration
{
    public class MultDbDataAttribute : DataAttribute
    {
        private readonly object inputData;

        public MultDbDataAttribute(object inputData=null)
        {
            this.inputData = inputData;
        }
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return this.inputData != null ? new List<object[]>
            {
                new object[]{ DatabaseProviderName.Sqlserver, this.inputData },
                //new object[]{ DatabaseProviderName.Oracle, this.inputData },
                //new object[]{ DatabaseProviderName.MySql,this.inputData },
            }:
             new List<object[]>
            {
                new object[]{ DatabaseProviderName.Sqlserver },
                //new object[]{ DatabaseProviderName.Oracle, this.inputData },
                //new object[]{ DatabaseProviderName.MySql,this.inputData },
            }
            ;
        }
    }
}
