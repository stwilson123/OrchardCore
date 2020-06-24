using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Navigation.Abstractions
{
    public class NavigationConfig
    {
        public IList<NavigationConfigItem> Items { get; set; }
    }
}
