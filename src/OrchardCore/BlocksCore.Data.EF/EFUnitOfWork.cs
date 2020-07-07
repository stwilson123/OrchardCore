using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Extensions;
using BlocksCore.Autofac.Extensions.DependencyInjection;
using BlocksCore.Autofac.Extensions.DependencyInjection.Paramters;
using BlocksCore.Data.Abstractions.Entities;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.EF
{
    public class EFUnitOfWork : IUnitOfWork
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

        public EFUnitOfWork(IServiceProvider serviceProvider, IDbConnectionAccessor dbConnectionAccessor, ITypeFeatureExtensionsProvider typeFeatureExtensionsProvider)
        {
            _serviceProvider = serviceProvider;
            _dbConnectionAccessor = dbConnectionAccessor;
            this._typeFeatureExtensionsProvider = typeFeatureExtensionsProvider;

        }

        public IDataContext GetOrCreateDataContext<TEntity>() where TEntity : IEntity
        {
            var lists = _typeFeatureExtensionsProvider.GetFeatureExportedTypesDenepencies(typeof(TEntity));
            return _serviceProvider.GetService<BlocksDbContext>(new NamedParam("entityTypes", lists));
        }

        public void Begin(UnitOfWorkOptions options)
        {
            _dbTransaction = DbConnection.BeginTransaction(options.IsolationLevel ?? IsolationLevel.ReadCommitted);
        }


        public void Complete()
        {
            if (_dbTransaction != null)
                _dbTransaction.Commit();
            _dbTransaction = null;
        }

        public Task CompleteAsync()
        {
            if (_dbTransaction != null)
                _dbTransaction.Commit();
            _dbTransaction = null;
            return Task.CompletedTask;
        }

        public void Rollback()
        {
            if (_dbTransaction != null)
                _dbTransaction.Rollback();
            _dbTransaction = null;
        }
    }
}