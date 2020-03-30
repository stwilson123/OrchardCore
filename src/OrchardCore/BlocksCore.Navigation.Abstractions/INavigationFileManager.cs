using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions;

namespace BlocksCore.Navigation.Abstractions
{
    public interface INavigationFileManager
    {
        Task Initialize();

        IDictionary<Platform, NavigationConfig> NavigationConfigs { get; }
    }
}
