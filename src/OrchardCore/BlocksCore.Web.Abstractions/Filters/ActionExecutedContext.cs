using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Filters
{
    public class ActionExecutedContext : FilterContext
    {
        IDictionary<string, object> ActionArguments { get; }

        public object Controller { get; }
       
        public object Result { get; set; }

        public ActionExecutedContext(object controller, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Controller = controller;
        }
    }
}
