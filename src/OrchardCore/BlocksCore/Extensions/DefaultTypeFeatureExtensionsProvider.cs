using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Extensions;
using OrchardCore.Environment.Extensions;

namespace BlocksCore.Extensions
{
    public class DefaultTypeFeatureExtensionsProvider : ITypeFeatureExtensionsProvider
    {
        private readonly IExtensionManager extensionManager;
        private readonly ITypeFeatureProvider typeFeatureProvider;

        public DefaultTypeFeatureExtensionsProvider(IExtensionManager extensionManager,ITypeFeatureProvider typeFeatureProvider)
        {
            this.extensionManager = extensionManager;
            this.typeFeatureProvider = typeFeatureProvider;
        }
        public IEnumerable<Type> GetFeatureDenepenciesForDependency(Type dependency)
        {
            var features = typeFeatureProvider.GetFeatureForDependency(dependency);
            if (features == null)
                return Enumerable.Empty<Type>();

            return this.extensionManager.LoadFeaturesAsync(new string[] { features.Id }).Result.SelectMany(f => f.ExportedTypes);
        }
    }
}
