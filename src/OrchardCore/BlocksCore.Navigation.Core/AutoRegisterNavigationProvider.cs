using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Navigation.Abstractions;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;

namespace BlocksCore.Navigation.Core
{
    public class AutoRegisterNavigationProvider : INavigationProvider
    {
        private readonly INavigationFileManager _navigationFileManager;
        public IStringLocalizer T { get; set; }

        public AutoRegisterNavigationProvider(INavigationFileManager navigationFileManager, IStringLocalizer<AutoRegisterNavigationProvider> localizer)
        {
            this._navigationFileManager = navigationFileManager;
            this.T = localizer;
        }
        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {

            foreach (var navigationConfig in _navigationFileManager.NavigationConfigs)
            {
                foreach (var item in navigationConfig.Items)
                {
                    builder.Add(T["Configuration"], "5", installed =>
                    {
                        var menuItem =
                        installed.Action(item.Action, item.ControllerName, item.AreaName)
                                 .LocalNav();
                        foreach (var permission in item.Permission)
                        {
                            menuItem = menuItem.Permission(new Permission(permission));
                        }
                    });
                }
            }
            return Task.CompletedTask;
        }
    }
}
