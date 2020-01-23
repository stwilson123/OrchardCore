using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlocksCore.Navigation.Abstractions
{
    public interface INavigationFileManager
    {
        Task Initialize();

        IEnumerable<NavigationConfig> NavigationConfigs { get; }
    }
}
