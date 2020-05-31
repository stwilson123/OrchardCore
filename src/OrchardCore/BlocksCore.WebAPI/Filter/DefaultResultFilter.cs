using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Web.Abstractions.Result;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.WebAPI.Filter
{
    public class DefaultResultFilter : IAlwaysRunResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var resultFilters = serviceProvider.GetServices<BlocksCore.Web.Abstractions.Filters.IResultFilter>();
            var orderedActionFilters = resultFilters.OrderBy(f => f is BlocksCore.Web.Abstractions.Filters.IOrderedFilter order ? order.Order : 0);
            foreach (var actionFilter in orderedActionFilters)
            {
                actionFilter.OnResultExecuted(new BlocksCore.Web.Abstractions.Filters.ResultExecutedContext(context.Controller, context.Result)
                {
                    Canceled = context.Canceled,
                    Exception = context.Exception,
                    ExceptionHandled = context.ExceptionHandled
                }); 
            }

          //  context.Result = ResultFactory.CreateDataResult(context.Result, context.Exception);
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var resultFilters = serviceProvider.GetServices<BlocksCore.Web.Abstractions.Filters.IResultFilter>();
            var orderedActionFilters = resultFilters.OrderBy(f => f is BlocksCore.Web.Abstractions.Filters.IOrderedFilter order ? order.Order : 0);
            foreach (var actionFilter in orderedActionFilters)
            {
                actionFilter.OnResultExecuting(new BlocksCore.Web.Abstractions.Filters.ResultExecutingContext(context.Controller)
                {
                    Cancel = context.Cancel,
                    Result = context.Result
                }); ;
            }
        }
    }
}
