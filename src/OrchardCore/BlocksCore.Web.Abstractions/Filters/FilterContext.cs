using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace BlocksCore.Web.Abstractions.Filters
{
    public abstract class FilterContext
    {
        protected FilterContext(IServiceProvider serviceProvider, TypeInfo controllerType)
        {
            ServiceProvider = serviceProvider;
            ControllerType = controllerType;
        }

        public IServiceProvider ServiceProvider { get; }

        public TypeInfo ControllerType { get; }

        public MethodInfo MethodInfo { get; set; }

        public HttpContext HttpContext { get; set; }

    }
}
