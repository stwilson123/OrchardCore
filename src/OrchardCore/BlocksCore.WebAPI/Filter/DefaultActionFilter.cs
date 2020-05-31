using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Web.Abstractions.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.WebAPI.Filter
{
    class DefaultActionFilter : IActionFilter, IOrderedFilter
    {

        public int Order { get; set; } = int.MaxValue - 10;

        public DefaultActionFilter()
        {


        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var actionFilters = serviceProvider.GetServices<BlocksCore.Web.Abstractions.Filters.IActionFilter>();
            var orderedActionFilters = actionFilters.OrderBy(f => f is BlocksCore.Web.Abstractions.Filters.IOrderedFilter order ? order.Order : 0);
            foreach (var actionFilter in orderedActionFilters)
            {
                actionFilter.OnActionExecuting(new BlocksCore.Web.Abstractions.Filters.ActionExecutingContext(context.ActionArguments));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;

            var actionFilters = serviceProvider.GetServices<BlocksCore.Web.Abstractions.Filters.IActionFilter>();
            var orderedActionFilters = actionFilters.OrderBy(f => f is BlocksCore.Web.Abstractions.Filters.IOrderedFilter order ? order.Order : 0);
            if (!(context.Result is ObjectResult objectResult))
                return;
            foreach (var actionFilter in orderedActionFilters)
            {

                var actionContext = new BlocksCore.Web.Abstractions.Filters.ActionExecutedContext(context.Controller, serviceProvider) { Result = objectResult.Value };
                actionFilter.OnActionExecuted(actionContext);

                objectResult.Value = actionContext.Result;
            }

            context.Result = new ObjectResult(ResultFactory.CreateDataResult(objectResult.Value, context.Exception));
        }


    }
}
