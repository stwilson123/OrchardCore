using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Extensions;
using BlocksCore.Data.Abstractions.Entities;
using BlocksCore.Data.Linq2DB.DBContext;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.Linq2DB
{
    public class Linq2DbUnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDbConnectionAccessor _dbConnectionAccessor;
        private readonly ITypeFeatureExtensionsProvider _typeFeatureExtensionsProvider;
        private IDbTransaction _dbTransaction;
        public DbConnection DbConnection
        {
            get
            {
                if (_dbConnection == null)
                    _dbConnection = _dbConnectionAccessor.CreateConnection();
                return _dbConnection;
            }
        }
        private DbConnection _dbConnection;

        public Linq2DbUnitOfWork(IServiceProvider serviceProvider, IDbConnectionAccessor dbConnectionAccessor, ITypeFeatureExtensionsProvider typeFeatureExtensionsProvider)
        {
            _serviceProvider = serviceProvider;
            _dbConnectionAccessor = dbConnectionAccessor;
            this._typeFeatureExtensionsProvider = typeFeatureExtensionsProvider;

        }

        public IDataContext GetOrCreateDataContext<TEntity>() where TEntity : IEntity
        {
            return _serviceProvider.GetService<BlocksDbContext>();
        }

        public void Begin(UnitOfWorkOptions options)
        {
            if (DbConnection.State == ConnectionState.Closed)
                DbConnection.Open();
            _dbTransaction = DbConnection.BeginTransaction(options.IsolationLevel ?? IsolationLevel.ReadCommitted);
        }


        public void Complete()
        {
            if (_dbTransaction != null)
                _dbTransaction.Commit();
            _dbTransaction = null;
            _dbConnection.Dispose();
        }

        public Task CompleteAsync()
        {
            Complete();
            return Task.CompletedTask;
        }

        public void Rollback()
        {
            if (_dbTransaction != null)
                _dbTransaction.Rollback();
            _dbTransaction = null;
            _dbConnection.Dispose();
            _dbConnection = null;
        }
    }
}