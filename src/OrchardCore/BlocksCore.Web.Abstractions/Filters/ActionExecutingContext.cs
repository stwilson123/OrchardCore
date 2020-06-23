using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BlocksCore.Web.Abstractions.Filters
{
    public class ActionExecutingContext : FilterContext
    {
        public IDictionary<string, object> ActionArguments { get; }
 

        public ActionExecutingContext(IDictionary<string, object> actionArguments, IServiceProvider serviceProvider, TypeInfo controllerType) : base(serviceProvider, controllerType)
        {
            ActionArguments = actionArguments;
         
        }
    }
}
