using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Navigation.Abstractions
{
    public interface INavigationFilter
    {
        IEnumerable<NavigationItem> OnFilterExecuting(IEnumerable<NavigationItem> navigationItems);


        IEnumerable<NavigationItem> OnFilterExecuted(IEnumerable<NavigationItem> navigationItems);

    }

}
