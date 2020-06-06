using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Domain.Abstractions;

namespace BlocksCore.Data.Core.UnitOfWork
{
    public class DefaultCurrentUnitOfWorkProvider : ICurrentUnitOfWorkProvider
    {
      
        public IUnitOfWork Current { get ; }
        public DefaultCurrentUnitOfWorkProvider(IUnitOfWork unitOfWork)
        {
            Current = unitOfWork;
        }
    }
}
