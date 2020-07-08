using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Infrastructure.Abstractions.Security.Permissions;
using BlocksCore.Navigation.Abstractions;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Navigation;

namespace BlocksCore.Navigation.Core
{
    public static class NavigationManagerExtensions
    {
        /// <summary>
        /// TODO only support two level filter the same menus
        /// </summary>
        /// <param name="navigation"></param>
        /// <param name="name"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<NavigationItem>> GetFilterMenuAsync(this INavigationManager navigation, string name, ActionContext context)
        {

            var menus = await navigation.BuildMenuAsync(name, context);


            return menus.Distinct(new MenuItemEqualityComparer()).Select(m => new NavigationItem(m.Id)
            {
                DisplayName = m.Text,
                Order = m.Priority,
                RouteValues = m.RouteValues,
                Permissions = m.Permissions.Select(p => p.ToPermision()).ToArray()
            });
        }
    }


    class MenuItemEqualityComparer : IEqualityComparer<MenuItem>
    {
        public bool Equals([AllowNull] MenuItem x, [AllowNull] MenuItem y)
        {
            return x == y;
        }

        public int GetHashCode([DisallowNull] MenuItem obj)
        {
            return obj.GetHashCode();
        }
    }
}
