using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.WebAPI.Filter
{
    class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {

        public int Order { get; set; } = int.MaxValue - 10;

        public HttpResponseExceptionFilter()
        {

        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var actionFilters = serviceProvider.GetServices<BlocksCore.Web.Abstractions.Filters.IActionFilter>();
            var orderedActionFilters = actionFilters.OrderBy(f => f is BlocksCore.Web.Abstractions.Filters.IOrderedFilter order ? order.Order : 100);
            var controllerType = FilterHelper.GetControllActionDescriptor(context)?.ControllerTypeInfo;
            foreach (var actionFilter in orderedActionFilters)
            {
                actionFilter.OnActionExecuting(new BlocksCore.Web.Abstractions.Filters.ActionExecutingContext(context.ActionArguments, serviceProvider, controllerType)
                {
                    HttpContext = context.HttpContext
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;

            var actionFilters = serviceProvider.GetServices<BlocksCore.Web.Abstractions.Filters.IActionFilter>();
            var orderedActionFilters = actionFilters.OrderBy(f => f is BlocksCore.Web.Abstractions.Filters.IOrderedFilter order ? order.Order : 100);
            var controllerType = FilterHelper.GetControllActionDescriptor(context)?.ControllerTypeInfo;
            foreach (var actionFilter in orderedActionFilters)
            {
                if (context.Result is ObjectResult objectResult)
                {
                    var actionContext = new BlocksCore.Web.Abstractions.Filters.ActionExecutedContext(serviceProvider, controllerType, context.Controller)
                    {
                        HttpContext = context.HttpContext,
                        Result = objectResult.Value
                    };
                    actionFilter.OnActionExecuted(actionContext);

                    objectResult.Value = actionContext.Result;
                }


            }

            //if (context.Exception is HttpResponseException exception)
            //{
            //    context.Result = new ObjectResult(exception.Value)
            //    {
            //        StatusCode = exception.Status,
            //    };
            //    context.ExceptionHandled = true;
            //}
        }
    }
}
