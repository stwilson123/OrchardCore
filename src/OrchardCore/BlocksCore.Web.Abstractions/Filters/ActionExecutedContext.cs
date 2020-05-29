using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Filters
{
    public class ActionExecutedContext
    {
        IDictionary<string, object> ActionArguments { get; }

        public object Controller { get; }
        public IServiceProvider ServiceProvider { get; }
        public object Result { get; set; }

        public ActionExecutedContext(object controller,IServiceProvider serviceProvider)
        {
            Controller = controller;
            ServiceProvider = serviceProvider;
        }
    }
}
