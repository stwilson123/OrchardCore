using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BlocksCore.Data.Abstractions.Transaction;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Web.Abstractions.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.Transaction
{
    public class TransactionExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.ControllerType.GetCustomAttribute<TransactionAttribute>()?.IsTransaction == false)
                return;
            var unitOfWorkManager = context.ServiceProvider.GetService<IUnitOfWorkManager>();

            unitOfWorkManager.Current.Rollback();
        }
    }
}
