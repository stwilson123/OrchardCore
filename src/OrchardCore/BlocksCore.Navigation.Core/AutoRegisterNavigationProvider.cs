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
        private readonly IStringLocalizer T;

        public AutoRegisterNavigationProvider(INavigationFileManager navigationFileManager, IStringLocalizer<AutoRegisterNavigationProvider> stringLocalizer)
        {
            this._navigationFileManager = navigationFileManager;
            this.T = stringLocalizer;
        }
        public async Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {

            await this._navigationFileManager.Initialize();
            foreach (var navigationConfig in _navigationFileManager.NavigationConfigs.Where(n => n.Key.ToString() == name && n.Value != null))
            {
                builder.Add(T[name], installed =>
                {
                    foreach (var item in navigationConfig.Value.Items)
                    {

                        installed.Add(T[item.Name], itemBuilder =>
                        {
                            var menuItem = itemBuilder.Action(item.Action, item.ControllerName, item.AreaName)
                                 .LocalNav();
                            foreach (var permission in item.Permission)
                            {
                                menuItem = menuItem.Permission(new Permission(permission));
                            }
                        });


                    }
                });
            }
            //return Task.CompletedTask;
        }
    }
}
