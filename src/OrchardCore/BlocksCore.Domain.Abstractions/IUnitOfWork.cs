using System.Data;
using System.Data.Common;

namespace BlocksCore.Domain.Abstractions
{
    public interface IUnitOfWork : IUnitOfWorkCompleteHandle
    {
        IDataContext GetOrCreateDataContext<TEntity>() where TEntity : IEntity;

        IDbConnection DbConnection { get; }

        IDbTransaction DbTransaction { get; }

        void Begin(UnitOfWorkOptions options);

        void Rollback();
    }
}