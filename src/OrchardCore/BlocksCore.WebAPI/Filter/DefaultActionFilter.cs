using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Web.Abstractions.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.WebAPI.Filter
{
    class DefaultActionFilter : Microsoft.AspNetCore.Mvc.Filters.IActionFilter, Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter
    {

        public int Order { get; set; } = int.MaxValue - 10;

        public DefaultActionFilter()
        {


        }

        public void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var actionFilters = serviceProvider.GetServices<BlocksCore.Web.Abstractions.Filters.IActionFilter>();
            var orderedActionFilters = actionFilters.OrderBy(f => f is BlocksCore.Web.Abstractions.Filters.IOrderedFilter order ? order.Order : 0);
            foreach (var actionFilter in orderedActionFilters)
            {
                actionFilter.OnActionExecuting(new BlocksCore.Web.Abstractions.Filters.ActionExecutingContext(context.ActionArguments, serviceProvider, ((ControllerActionDescriptor)context.ActionDescriptor)?.ControllerTypeInfo));
            }
        }

        public void OnActionExecuted(Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;

            var actionFilters = serviceProvider.GetServices<BlocksCore.Web.Abstractions.Filters.IActionFilter>();
            var orderedActionFilters = actionFilters.OrderBy(f => f is BlocksCore.Web.Abstractions.Filters.IOrderedFilter order ? order.Order : 0);
            var objectHandleResult = FilterHelper.IsObjectResult(context.Result, context.ActionDescriptor);
            var resultObj = resultHandle(context, serviceProvider, orderedActionFilters, objectHandleResult.Result);
            context.Result = objectHandleResult.IsObjectResult ? new ObjectResult(ResultFactory.CreateDataResult(resultObj, context.Exception)) : resultObj as IActionResult;
        }

        private static object resultHandle(Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext context, IServiceProvider serviceProvider, IOrderedEnumerable<Web.Abstractions.Filters.IActionFilter> orderedActionFilters, object resultObj)
        {
            foreach (var actionFilter in orderedActionFilters)
            {
                var actionContext = new BlocksCore.Web.Abstractions.Filters.ActionExecutedContext(serviceProvider, ((ControllerActionDescriptor)context.ActionDescriptor)?.ControllerTypeInfo) { Result = resultObj };
                actionFilter.OnActionExecuted(actionContext);
                resultObj = actionContext.Result;
            }
            return resultObj;
        }
    }
}
