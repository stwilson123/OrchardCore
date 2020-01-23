using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlocksCore.Abstractions;
using BlocksCore.Abstractions.extensions;
using BlocksCore.Navigation.Abstractions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;
using OrchardCore.Modules.FileProviders;

namespace BlocksCore.Navigation.Core
{
    public class NavigationFileManager : INavigationFileManager
    {
        public NavigationFileManager(IEnumerable<INavigationFileProvider> navigationFileProviders, IHostEnvironment hostingEnvironment, IExtensionManager extensionManager, ITypeFeatureProvider typeFeatureProvider )
        {
            this._navigationFileProviders = navigationFileProviders;
            this._extensionManager = extensionManager;
            this._typeFeatureProvider = typeFeatureProvider;
       
            _fileProvider = hostingEnvironment.ContentRootFileProvider;
        }
        private readonly IEnumerable<INavigationFileProvider> _navigationFileProviders;
        private readonly IExtensionManager _extensionManager;
        private readonly ITypeFeatureProvider _typeFeatureProvider;
        private IFileProvider _fileProvider;

        public IEnumerable<NavigationConfig> NavigationConfigs => throw new NotImplementedException();

        private IList<NavigationConfig> _navigationConfigs = new List<NavigationConfig>();

        public async Task  Initialize()
        {

            foreach (var navigationFileProvider in _navigationFileProviders.Where(p => p.filePaths != null && p.filePaths.Any()))
            {
                var feature = _typeFeatureProvider.GetFeatureForDependency(navigationFileProvider.GetType());
                foreach (var navigationFilePath in navigationFileProvider.filePaths)
                {
                   var fileProvider =  _fileProvider.GetFileInfo(PathExtensions.Combine(feature.Extension.SubPath, "Navigation", navigationFilePath.Value));
                    if (!fileProvider.Exists)
                        continue;
                   var JsonString =  await fileProvider.ReadToString();

                   var navgationConfig = JsonConvert.DeserializeObject<NavigationConfig>(JsonString);
                    _navigationConfigs.Add(navgationConfig);
                }
                  
            }
             
             



        }
    }
}
