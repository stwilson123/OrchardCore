using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Extensions;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Modules.Manifest;

namespace BlocksCore.Extensions
{
    public class DefaultTypeFeatureExtensionsProvider : ITypeFeatureExtensionsProvider
    {
        private readonly IExtensionManager extensionManager;
        private readonly ITypeFeatureProvider typeFeatureProvider;
        private bool _isInitialized = false;
        private static object InitializationSyncLock = new object();
        private IEnumerable<IHasChildNames> _mainFeatures;
        private IDictionary<string, IHasChildNames> _dicChildMapMainFeature = new Dictionary<string, IHasChildNames>();
        public DefaultTypeFeatureExtensionsProvider(IExtensionManager extensionManager,ITypeFeatureProvider typeFeatureProvider)
        {
            this.extensionManager = extensionManager;
            this.typeFeatureProvider = typeFeatureProvider;
        }
        public IEnumerable<Type> GetFeatureExportedTypesDenepencies(Type dependency)
        {
            var features = typeFeatureProvider.GetFeatureForDependency(dependency);
            if (features == null)
                return Enumerable.Empty<Type>();

            return this.extensionManager.LoadFeaturesAsync(GetMainFeature(features).Item2).Result.SelectMany(f => f.ExportedTypes);
        }

        public IFeatureInfo GetMainFeatureForDependency(Type dependency)
        {
            var feature = typeFeatureProvider.GetFeatureForDependency(dependency);
            //feature.Extension.
            //return this.extensionManager.LoadFeaturesAsync(new string[] { features.Id }).Result.SelectMany(f => f.ExportedTypes);
            return GetMainFeature(feature).Item1;
        }

        private void EnsureInitialized()
        {
            if (_isInitialized)
                return;
            lock(InitializationSyncLock)
            {
                _mainFeatures = this.extensionManager.GetExtensions().SelectMany(e =>
                {
                    var allMainFeature = new List<IHasChildNames>();
                    if (e.Manifest.ModuleInfo is IHasChildNames mainFeature)
                        allMainFeature.Add(mainFeature);
                    allMainFeature.AddRange(e.Manifest.ModuleInfo.Features.OfType<IHasChildNames>());
                    return allMainFeature;
                })
                .Where(f => f.ChildNames != null).ToList();
                foreach (var mainFeature in _mainFeatures)
                {
                    foreach (var childName in mainFeature.ChildNames)
                    {
                        _dicChildMapMainFeature.Add(childName, mainFeature);

                    }
                }
            }
        

            _isInitialized = true;
        }

        private (IFeatureInfo, string[]) GetMainFeature(IFeatureInfo feature)
        {
            //TODO need to cache?
            EnsureInitialized();
            IHasChildNames mainFeature;
            if (!_dicChildMapMainFeature.TryGetValue(feature.Name,out mainFeature))
            {
                return (feature,new[] { feature.Name });
            }


            return (this.extensionManager.GetFeatures(new[] { mainFeature.Id }).First(), mainFeature.ChildNames);
        }
    }
}
