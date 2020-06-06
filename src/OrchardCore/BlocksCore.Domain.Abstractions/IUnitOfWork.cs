using System.Data;
using System.Data.Common;

namespace BlocksCore.Domain.Abstractions
{
    public interface IUnitOfWork : IUnitOfWorkCompleteHandle
    {
        IDataContext GetOrCreateDataContext<TEntity>() where TEntity : IEntity;

        DbConnection DbConnection { get; }

        void Begin(UnitOfWorkOptions options);

        void Rollback();
    }
}