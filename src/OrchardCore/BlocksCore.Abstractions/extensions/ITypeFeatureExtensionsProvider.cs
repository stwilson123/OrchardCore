using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlocksCore.Abstractions.Extensions
{
    public interface ITypeFeatureExtensionsProvider
    {
        IEnumerable<Type> GetFeatureDenepenciesForDependency(Type dependency);
    }
}
