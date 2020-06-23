using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Data.Abstractions.Transaction
{
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Method)]
    public class TransactionAttribute : Attribute
    {
        public TransactionAttribute(bool isTransaction = true)
        {
            IsTransaction = isTransaction;
        }

        public bool IsTransaction { get; }
    }
}
