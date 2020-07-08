using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Infrastructure.Abstractions.Security.Permissions;
using Microsoft.Extensions.Localization;

namespace BlocksCore.Navigation.Abstractions
{
    public class NavigationItem
    {
        public string Name { get;  }

   

        public LocalizedString DisplayName { get; set; }

        //public string Action { get; set; }
        //public string ControllerName { get; set; }
        //public string AreaName { get; set; }

        public IDictionary<string, object> RouteValues { get; set; }
        public int? NavigationType { get; set; }
        public Permission[] Permissions { get; set; }

        public bool IsVisible { get; set; }

        public string uId { get => GetUniqueId(this); }

        public long Order { get; set; }

        public string Icon { get; set; }

        public IList<NavigationItem> Items { get; set; }
        public NavigationItem(string name)
        {
            Name = name;
            Items = new List<NavigationItem>();
        }
        static string GetUniqueId(NavigationItem navigationItem)
        {
            return navigationItem.Name + "_" + navigationItem.RouteValues?["area"];
        }


    }
}
