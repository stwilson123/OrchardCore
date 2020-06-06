using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.Core.UnitOfWork
{
    public class DefaultUnitOfWorkManager : IUnitOfWorkManager
    {

        public IUnitOfWork Current { get; }

        public DefaultUnitOfWorkManager(IUnitOfWork unitOfWork)
        {
            Current = unitOfWork;
        }

        public IUnitOfWorkCompleteHandle Begin()
        {
            return Begin(new UnitOfWorkOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(30) });
        }

        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            Current.Begin(options);

            return Current;
        }
    }
}
