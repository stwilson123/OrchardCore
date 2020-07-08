using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BlocksCore.Navigation.Abstractions
{
    public class NavigationConfig
    {
        public IList<NavigationConfigItem> Items { get; set; }

       
    }

    public static class NavigationConfigExtension
    {
        public static void Merge(this NavigationConfig config,NavigationConfig otherConfig)
        {
            if (config == null || otherConfig == null || otherConfig.Items == null)
                return;


            if (config.Items == null)
                config.Items = new List<NavigationConfigItem>();

            foreach (var item in otherConfig.Items)
            {
                config.Items.Add(item);
            }
        }
    }
    
}
