using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Extensions.Features;

namespace BlocksCore.Test.Navigation
{
    public class DefaultTypeFeatureProvider : ITypeFeatureProvider
    {
        public IFeatureInfo GetFeatureForDependency(Type dependency)
        {
            return new FeatureInfo("BlocksCore.Test", "TestModule", 0, null, null, new ExtensionInfo("", null,(a,b) => null ),null,false);
        }

        public void TryAdd(Type type, IFeatureInfo feature)
        {
           // throw new NotImplementedException();
        }
    }
}
