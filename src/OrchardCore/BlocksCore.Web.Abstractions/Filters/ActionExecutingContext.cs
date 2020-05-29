using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Filters
{
    public class ActionExecutingContext
    {
        IDictionary<string, object> ActionArguments { get; }

        public ActionExecutingContext(IDictionary<string, object> actionArguments)
        {
            ActionArguments = actionArguments;
        }
    }
}
