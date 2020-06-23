using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BlocksCore.Web.Abstractions.Filters
{
    public class ActionExecutedContext : FilterContext
    {
        IDictionary<string, object> ActionArguments { get; }

    
       
        public object Result { get; set; }

        public ActionExecutedContext(IServiceProvider serviceProvider, TypeInfo controllerType) : base(serviceProvider, controllerType)
        {
        }
    }
}
