using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Web.Abstractions.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.Transaction
{
    public class TransactionExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var unitOfWorkManager = context.ServiceProvider.GetService<IUnitOfWorkManager>();

            unitOfWorkManager.Current.Rollback();
        }
    }
}
