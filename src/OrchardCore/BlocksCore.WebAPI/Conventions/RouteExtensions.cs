using System;
using Microsoft.AspNetCore.Routing;

namespace BlocksCore.WebAPI.Conventions
{
    public static class RouteExtensions
    {
        public static string GetAreaName(this RouteBase route)
        {
 
            var castRoute = route as Microsoft.AspNetCore.Routing.Route;
            if (castRoute != null && castRoute.DataTokens != null)
            {
                return castRoute.DataTokens["area"] as string;
            }

            return null;
        }

        public static string GetAreaName(this RouteData routeData)
        {
            object area;
            if (routeData.Values.TryGetValue("area", out area))
            {
                return area as string;
            }

            if (routeData.DataTokens.TryGetValue("area", out area))
            {
                return area as string;
            }
            return String.Empty;
             
            //return GetAreaName(routeData);
        }
        
        public static string GetControllerName(this RouteData routeData)
        {
            object area;
            if (routeData.Values.TryGetValue("controller", out area))
            {
                return area as string;
            }
            if (routeData.DataTokens.TryGetValue("controller", out area))
            {
                return area as string;
            }
            return String.Empty;
             
            //return GetAreaName(routeData);
        }
    }
}