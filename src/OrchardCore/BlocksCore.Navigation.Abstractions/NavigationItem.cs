using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Infrastructure.Abstractions.Security.Permissions;
using Microsoft.Extensions.Localization;

namespace BlocksCore.Navigation.Abstractions
{
    public class NavigationItem
    {
        public string Name { get; set; }
        public LocalizedString DisplayName { get; set; }

        public string Action { get; set; }
        public string ControllerName { get; set; }
        public string AreaName { get; set; }
        public int? NavigationType { get; set; }
        public Permission[] Permission { get; set; }

        public bool IsVisible { get; set; }
        public string GetUniqueId()
        {
            return Name + "_" + AreaName;
        }
    }
}
