using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Domain.Abstractions;
using BlocksCore.Web.Abstractions.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.Transaction
{
    public class TransactionActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
             var unitOfWorkManager = context.ServiceProvider.GetService<IUnitOfWorkManager>();

            unitOfWorkManager.Current.Complete();

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var unitOfWorkManager = context.ServiceProvider.GetService<IUnitOfWorkManager>();
            unitOfWorkManager.Begin();
        }
    }
}
