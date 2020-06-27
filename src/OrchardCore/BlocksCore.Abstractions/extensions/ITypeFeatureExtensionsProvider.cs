using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.Environment.Extensions.Features;

namespace BlocksCore.Abstractions.Extensions
{
    public interface ITypeFeatureExtensionsProvider
    {
        IEnumerable<Type> GetFeatureExportedTypesDenepencies(Type dependency);

        IFeatureInfo GetMainFeatureForDependency(Type dependency);
    }
}
