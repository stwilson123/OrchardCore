using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions;
using BlocksCore.Navigation.Abstractions;

namespace Blocks.BussnessWebModule.Config.Navigation
{
    public class NavigationFileProvider : INavigationFileProvider
    {
        public IDictionary<Platform, string> filePaths => new Dictionary<Platform, string> {
            { Platform.Main, "Config/Navigation/WebNavigation.json" }
        };
    }
}
