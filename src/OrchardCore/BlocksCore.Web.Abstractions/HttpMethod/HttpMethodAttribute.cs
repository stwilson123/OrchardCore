using System;

namespace BlocksCore.Web.Abstractions.HttpMethod
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class HttpMethodAttribute : Attribute
    {
        private readonly HttpVerb _httpVerb;

        public HttpMethodAttribute(HttpVerb httpVerb)
        {
            _httpVerb = httpVerb;
        }
        public HttpVerb HttpMethod
        {
            get
            {
                return _httpVerb;
            }
        }
    }
}