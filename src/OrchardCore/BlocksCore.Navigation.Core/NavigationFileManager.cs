using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlocksCore.Abstractions;
using BlocksCore.Abstractions.Extensions;
using BlocksCore.Navigation.Abstractions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;
using OrchardCore.Modules.FileProviders;
using BlocksCore.SyntacticAbstractions.Types.Collections;
using Microsoft.Extensions.Localization;
using BlocksCore.Localization.Abtractions;

namespace BlocksCore.Navigation.Core
{
    public class NavigationFileManager : INavigationFileManager
    {
        public NavigationFileManager(IEnumerable<INavigationFileProvider> navigationFileProviders, IHostEnvironment hostingEnvironment, ITypeFeatureProvider typeFeatureProvider, IStringLocalizerFactory stringLocalizerFactory)
        {
            this._navigationFileProviders = navigationFileProviders;
            this._typeFeatureProvider = typeFeatureProvider;
            this._stringLocalizerFactory = stringLocalizerFactory;
            _fileProvider = hostingEnvironment.ContentRootFileProvider;
        }
        private readonly IEnumerable<INavigationFileProvider> _navigationFileProviders;
        private readonly ITypeFeatureProvider _typeFeatureProvider;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;
        private IFileProvider _fileProvider;

        public IDictionary<Platform, NavigationConfig> NavigationConfigs => _navigationConfigs;

        private IDictionary<Platform, NavigationConfig> _navigationConfigs = new Dictionary<Platform, NavigationConfig>();

        public async Task Initialize()
        {
            if (_navigationConfigs.Any())
                return;
            foreach (var navigationFileProvider in _navigationFileProviders.Where(p => p.filePaths != null && p.filePaths.Any()))
            {
                var feature = _typeFeatureProvider.GetFeatureForDependency(navigationFileProvider.GetType());
                foreach (var navigationFile in navigationFileProvider.filePaths)
                {
                    var fileProvider = _fileProvider.GetFileInfo(PathExtensions.Combine(feature.Extension.SubPath,navigationFile.Value));
                    if (!fileProvider.Exists)
                        continue;

                    var JsonString = await fileProvider.ReadToString();

                    var navgationConfig = JsonConvert.DeserializeObject<NavigationConfig>(JsonString);

                    if (navgationConfig != null && !navgationConfig.Items.IsNullOrEmpty())
                    {
                        foreach (var navItem in navgationConfig.Items)
                        {
                            navItem.SourceName = feature.Name;
                            navItem.AreaName = string.IsNullOrEmpty(navItem.AreaName) ? feature.Name : navItem.AreaName;
                        }
                    }

                    if(_navigationConfigs.TryGetValue(navigationFile.Key,out NavigationConfig navigationConfigExist))
                    {
                        navigationConfigExist.Merge(navgationConfig);
                    }
                    else
                    {
                        _navigationConfigs.Add(navigationFile.Key, navgationConfig);

                    }
                }

            }





        }
    }
}
