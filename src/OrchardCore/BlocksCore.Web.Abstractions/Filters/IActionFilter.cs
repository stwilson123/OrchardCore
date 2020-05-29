using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Filters
{
    public interface IActionFilter
    {
        void OnActionExecuting(ActionExecutingContext context);

        void OnActionExecuted(ActionExecutedContext context);
    }
}
