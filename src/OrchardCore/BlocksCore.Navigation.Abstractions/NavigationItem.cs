using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Navigation.Abstractions
{
    public class NavigationItem
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Action { get; set; }
        public string ControllerName { get; set; }
        public string AreaName { get; set; }
        public int? NavigationType { get; set; }
        public string[] Permission { get; set; }

        public string Url {
            get {
                return $"{AreaName ?? ""}/{ControllerName ?? ""}{Action ?? ""}"; 

            }
        }
    }
}
