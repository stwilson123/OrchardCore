using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Abstractions.Infrastructure;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;
namespace BlocksCore.Data.Core.Infrastructure
{
    public class DbContextServices : IDbContextServices
    {
        public IDataContext CurrentContext => _dataContext;

        public IModel Model => CreateModel();

        public IServiceProvider InternalServiceProvider => _scopedProvider;

        private IServiceProvider _scopedProvider;
        private IDbContextOptions _contextOptions;
        private IDataContext _dataContext;
       // private bool _inOnModelCreating;


        public IDbContextServices Initialize(IServiceProvider scopedProvider, IDbContextOptions contextOptions, IDataContext context)
        {
            _scopedProvider = scopedProvider;
            _contextOptions = contextOptions;
            _dataContext = context;
            return this;
        }

        private IModel CreateModel()
        {
            return new EntityModel(_dataContext.EntityTypes);
            //if (_inOnModelCreating)
            //{
            //    throw new InvalidOperationException("Model is creating.");
            //}

            //try
            //{
            //    _inOnModelCreating = true;

            //    return new EntityModel(_dataContext.EntityTypes);
            //}
            //finally
            //{
            //    _inOnModelCreating = false;
            //}
        }

        public ConnectionInfo GetConnetionInfo() => InternalServiceProvider.GetService<ConnectionInfo>();

        public IDbConnection CreateConnection(string connectionString)
        {
            return InternalServiceProvider.GetService<Func<string, IDbConnection>>()(connectionString);
        }

      
    }
}
