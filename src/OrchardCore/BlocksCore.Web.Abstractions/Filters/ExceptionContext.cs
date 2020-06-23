using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BlocksCore.Web.Abstractions.Filters
{
    public class ExceptionContext : FilterContext
    {
        public ExceptionContext(IServiceProvider serviceProvider, TypeInfo controllerType) : base(serviceProvider, controllerType)
        {
        }

        public Exception Exception { get; set; }

        public bool ExceptionHandled { get; set; }
        public object Result { get; set; }
    }
}
