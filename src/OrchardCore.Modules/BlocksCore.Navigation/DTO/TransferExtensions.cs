using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using BlocksCore.Navigation.Abstractions;
using BlocksCore.Web.Abstractions.Route;

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
                uId = menuItem.uId,
                Url = RouteHelper.GetUrl(menuItem.RouteValues),
                Items = menuItem.Items?.Select(i => ToMenuItemDTO(i))
            };
        }

        public static string GetUniqueId([DisallowNull]this OrchardCore.Navigation.MenuItem navItem)
        {
            return navItem.Text.Name + "_" + navItem.RouteValues?["area"];
        }


    }
}
