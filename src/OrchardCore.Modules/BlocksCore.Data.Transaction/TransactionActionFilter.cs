using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BlocksCore.Data.Abstractions.Transaction;
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
            if (context.ControllerType.GetCustomAttribute<TransactionAttribute>()?.IsTransaction == false)
                return;
             var unitOfWorkManager = context.ServiceProvider.GetService<IUnitOfWorkManager>();

            unitOfWorkManager.Current.Complete();

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ControllerType.GetCustomAttribute<TransactionAttribute>()?.IsTransaction == false)
                return;
            var unitOfWorkManager = context.ServiceProvider.GetService<IUnitOfWorkManager>();
            unitOfWorkManager.Begin();
        }
    }
}
