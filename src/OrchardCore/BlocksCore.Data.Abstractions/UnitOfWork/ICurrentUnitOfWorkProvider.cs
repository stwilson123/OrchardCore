using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Domain.Abstractions;

namespace BlocksCore.Data.Abstractions.UnitOfWork
{
    public interface ICurrentUnitOfWorkProvider
    {
        IUnitOfWork Current { get;  }
    }
}
