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
    public class DefaultExceptionFilter : Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter
    {
        public void OnException(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;

            var actionFilters = serviceProvider.GetServices<BlocksCore.Web.Abstractions.Filters.IExceptionFilter>();
            var orderedActionFilters = actionFilters.OrderBy(f => f is BlocksCore.Web.Abstractions.Filters.IOrderedFilter order ? order.Order : 0);
            var objectHandleResult = FilterHelper.IsObjectResult(context.Result, context.ActionDescriptor);
            var resultObj = resultHandle(context, serviceProvider, orderedActionFilters, objectHandleResult.Result);

            context.ExceptionHandled = true;

            context.Result = objectHandleResult.IsObjectResult ? new ObjectResult(ResultFactory.CreateDataResult(null, context.Exception))
                : resultObj as IActionResult;
        }

        private static object resultHandle(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext context, IServiceProvider serviceProvider, IOrderedEnumerable<Web.Abstractions.Filters.IExceptionFilter> orderedActionFilters, object resultObj)
        {
            foreach (var actionFilter in orderedActionFilters)
            {
                var actionContext = new BlocksCore.Web.Abstractions.Filters.ExceptionContext(serviceProvider, ((ControllerActionDescriptor)context.ActionDescriptor)?.ControllerTypeInfo) { Result = resultObj, Exception = context.Exception, ExceptionHandled = context.ExceptionHandled };
                actionFilter.OnException(actionContext);
                resultObj = actionContext.Result;
            }

            return resultObj;
        }
    }
}
