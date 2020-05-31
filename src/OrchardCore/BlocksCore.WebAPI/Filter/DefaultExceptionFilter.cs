using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Web.Abstractions.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlocksCore.WebAPI.Filter
{
    public class DefaultExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            context.Result = new ObjectResult(ResultFactory.CreateDataResult(null, context.Exception));
        }
    }
}
