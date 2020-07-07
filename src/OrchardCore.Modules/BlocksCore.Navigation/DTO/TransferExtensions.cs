using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using BlocksCore.Navigation.Abstractions;

namespace BlocksCore.Navigation.DTO
{
    public static class TransferExtensions
    {
        public static MenuItem ToMenuItemDTO([DisallowNull]this NavigationItem menuItem)
        {
            return new MenuItem()
            {
                Name = menuItem.Name,
                DisplayName = menuItem.DisplayName,
                Order = menuItem.Order,
                uId = menuItem.uId
            };
        }

        public static string GetUniqueId([DisallowNull]this OrchardCore.Navigation.MenuItem navItem)
        {
            return navItem.Text.Name + "_" + navItem.RouteValues?["area"];
        }


    }
}
