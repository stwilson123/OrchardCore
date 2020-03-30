using System;

namespace BlocksCore.Web.Abstractions.Route
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class HttpAreaAttribute : HttpRouteValueAttribute
    {
        public HttpAreaAttribute(string areaName)
            : base("area", areaName)
        {
            if (string.IsNullOrEmpty(areaName))
                throw new ArgumentException("Argument Cannot Be Null Or Empty", nameof (areaName));
        }
    }
}