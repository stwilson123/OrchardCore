using System;

namespace BlocksCore.Web.Abstractions.Route
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class HttpRouteValueAttribute : Attribute
    {
        protected HttpRouteValueAttribute(string routeKey, string routeValue)
        {
            if (routeKey == null)
                throw new ArgumentNullException(nameof (routeKey));
            if (routeValue == null)
                throw new ArgumentNullException(nameof (routeValue));
            this.RouteKey = routeKey;
            this.RouteValue = routeValue;
        }

        public string RouteKey { get; }

        public string RouteValue { get; }
    }
}