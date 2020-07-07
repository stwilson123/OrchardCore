using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlocksCore.Web.Abstractions.Route
{
    public class RouteHelper
    {
        public static string GetUrl(IDictionary<string, object> routeValue)
        {
            if (routeValue == null || !routeValue.Any())
                return "";
            var controllerServiceName = routeValue["area"]?.ToString() + "/" + routeValue["controller"]?.ToString()
                                        + "/" + routeValue["action"]?.ToString();
            return controllerServiceName.ToLower();
        }

        public static string GetControllerPath(IDictionary<string, object> routeValue)
        {
            var controllerServiceName = routeValue["area"]?.ToString() + "/" + routeValue["controller"]?.ToString();
            return controllerServiceName;
        }
    }
}
