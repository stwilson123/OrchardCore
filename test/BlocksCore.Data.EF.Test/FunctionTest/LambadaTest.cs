using System;
using System.Linq.Expressions;
using BlocksCore.Data.EF.Linq.Extends;
using Xunit;

namespace BlocksCore.Data.EF.Test.FunctionTest
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