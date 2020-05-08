using System;
using System.Data;
using System.Data.Common;
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
        public DbConnection DbConnection { get; private set; }

        public EFUnitOfWork(IServiceProvider serviceProvider, IDbConnectionAccessor dbConnectionAccessor, ITypeFeatureExtensionsProvider typeFeatureExtensionsProvider)
        {
            _serviceProvider = serviceProvider;
            _dbConnectionAccessor = dbConnectionAccessor;
            this._typeFeatureExtensionsProvider = typeFeatureExtensionsProvider;
            DbConnection = _dbConnectionAccessor.CreateConnection();
        }

        public IDataContext GetOrCreateDataContext<TEntity>() where TEntity : IEntity
        {
            var lists = _typeFeatureExtensionsProvider.GetFeatureDenepenciesForDependency(typeof(TEntity));
            return _serviceProvider.GetService<BlocksDbContext>(new NamedParam("entityTypes",lists));
        }

        public void Begin()
        {
            _dbTransaction = DbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void Commit()
        {
            if (_dbTransaction != null)
                _dbTransaction.Commit();
        }
    }
}