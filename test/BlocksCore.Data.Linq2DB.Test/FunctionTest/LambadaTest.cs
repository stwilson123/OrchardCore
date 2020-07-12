using System;
using System.Linq.Expressions;
using BlocksCore.Data.Extends;
using Xunit;

namespace BlocksCore.Data.Linq2DB.Test.FunctionTest
{
    public class LambadaTest
    {
        [Fact]
        public void DefaultConfigIsDetectChanges()
        {
            Expression<Func<LambadaTest, LambadaTest>> expression = (input) => new LambadaTest();
            var a = new { };
            ExpressionUtils.Convert(expression, a.GetType());
        }

    }
}