using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BlocksCore.Data.Abstractions.Transaction;
using BlocksCore.Web.Abstractions.Filters;

namespace BlocksCore.Data.Transaction
{
    class TransactionHelper
    {
        public static bool IsUseTransaction(FilterContext context)
        {
            var transactionAttribute = context.ControllerType.GetCustomAttribute<TransactionAttribute>();
            if(transactionAttribute != null)
            {
                return transactionAttribute.IsTransaction;
            }

            return context?.HttpContext?.Request?.Method.ToUpper() != "GET" ;
        }
    }
}
