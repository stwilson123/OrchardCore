using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static async Task<IEnumerable<MenuItem>> GetFilterMenuAsync(this INavigationManager navigation,string name, ActionContext context)
        {

            var menus = await navigation.BuildMenuAsync(name, context);
          
            return menus.Distinct(new MenuItemEqualityComparer());
        }
    }

    class MenuItemEqualityComparer : IEqualityComparer<MenuItem>
    {
        public bool Equals([AllowNull] MenuItem x, [AllowNull] MenuItem y)
        {
            return  x == y;
        }

        public int GetHashCode([DisallowNull] MenuItem obj)
        {
            return obj.GetHashCode();
        }
    }
}
