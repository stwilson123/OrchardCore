using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BlocksCore.Navigation.DTO
{
    public static class TransferExtensions
    {
        public static MenuItem ToMenuItemDTO([DisallowNull]this OrchardCore.Navigation.MenuItem menuItem)
        {
            return new MenuItem()
            {
                Name = menuItem.Text.Name,
                DisplayName = menuItem.Text.ToString(),
                Order = menuItem.Priority,
                uId = menuItem.GetUniqueId()
            };
        }

        public static string GetUniqueId([DisallowNull]this OrchardCore.Navigation.MenuItem navItem)
        {
            return navItem.Text.Name + "_" + navItem.RouteValues?["area"];
        }


    }
}
