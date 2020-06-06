using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Domain.Abstractions;

namespace BlocksCore.Data.Abstractions.UnitOfWork
{
    public interface IUnitOfWorkManager
    {

        IUnitOfWork Current { get; }

        IUnitOfWorkCompleteHandle Begin();

     
        IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options);
    }
}
