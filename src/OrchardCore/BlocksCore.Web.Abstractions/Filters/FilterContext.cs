using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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
    }
}
