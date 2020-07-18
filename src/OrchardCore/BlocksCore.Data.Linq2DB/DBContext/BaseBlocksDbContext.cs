using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Extensions;
using BlocksCore.Data.Abstractions;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Abstractions.Entities;
using BlocksCore.Data.Abstractions.Infrastructure;
using BlocksCore.Data.Abstractions.Migrator;
using BlocksCore.Data.Core.Services;
using BlocksCore.Domain.Abstractions;
using BlocksCore.SyntacticAbstractions.Reflection.Extensions;
using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;
using IDataContext = BlocksCore.Domain.Abstractions.IDataContext;

namespace BlocksCore.Data.Linq2DB
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// </summary>
    public abstract class BaseBlocksDbContext : DataConnection, IDataContext ///, ITransientDependency, IShouldInitialize
    {
     
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        protected ILogger Logger { get; set; }
        protected DbContextOption<LinqToDbConnectionOptions> dbContextOption { get; }

        /// <summary>
        /// Reference to the event bus.
        /// </summary>
        // public IEventBus EventBus { get; set; }

        /// <summary>
        /// Reference to GUID generator. TODO Create sequence id.
        /// </summary>
        // public IGuidGenerator GuidGenerator { get; set; }

        //private ITypeFeatureExtensionsProvider typeFeatureExtensionsProvider;


        /// <summary>
        /// Reference to the current UOW provider.
        /// </summary>
        // public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

        /// <summary>
        /// Reference to multi tenancy configuration.
        /// </summary>
        // public IMultiTenancyConfig MultiTenancyConfig { get; set; }


        private bool _initializing = false;
        public IServiceProvider InternalServiceProvider { get => GetServiceProvider(); }
        private IDbContextServices _contextServices;

        /// <summary>
        /// Can be used to suppress automatically setting TenantId on SaveChanges.
        /// Default: false.
        /// </summary>
        public bool SuppressAutoSetTenantId { get; set; }


        private ShellSettings _settingManager { get; set; }

        protected virtual bool isDbMigrate { get; set; } = false;

        public IEnumerable<Type> EntityTypes { get; }
        private readonly ServiceProviderCache serviceProviderCache;
        private IServiceScope _serviceScope;
        public IQueryable Get<TEntity>() where TEntity : class
        {
            return this.GetTable<TEntity>();
        }
        /// </summary>
        protected BaseBlocksDbContext(ShellSettings settingManager, ILogger log, string connectionString, DbContextOption<LinqToDbConnectionOptions> options) : base(connectionString)
        {
            _settingManager = settingManager;
            this.Logger = log;
            this.dbContextOption = options;
        }


        /// <summary>
        /// Constructor.
        /// Uses <see cref="IAbpStartupConfiguration.DefaultNameOrConnectionString"/> as connection string.
        /// </summary>
        protected BaseBlocksDbContext(IEnumerable<Type> entityTypes, ShellSettings settingManager, ILogger log, DbContextOption<LinqToDbConnectionOptions> options,ServiceProviderCache serviceProviderCache) : base(options.Option)
        {
            this.EntityTypes = entityTypes;
            _settingManager = settingManager;
            this.Logger = log;
            this.dbContextOption = options;
            this.serviceProviderCache = serviceProviderCache;
        }



        public bool EnsureCreated()
        {
            if (!isDbMigrate)
                return false;
            var a = this.GetServiceProvider().GetService < string>();
            this.GetServiceProvider().GetService<IDatabaseCreator>().EnsureCreated();

            foreach (var entity in GetDbEntities(this))
            {
                var createTableMethod = typeof(DataExtensions).GetMethod("CreateTable").MakeGenericMethod(entity);
                createTableMethod.Invoke(this, new object[] { this, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing });
            }  
            
            
            return isDbMigrate;
        }


        private IEnumerable<Type> GetDbEntities(BaseBlocksDbContext context)
        {

            var dbEntitits = EntityTypes.Where(p => !p.IsAbstract && !p.IsInterface && typeof(IEntity).IsAssignableFrom(p));
            return dbEntitits;
        }

        public bool EnsureDeleted()
        {
            if (!isDbMigrate)
                return false;
            this.GetServiceProvider().GetService<IDatabaseCreator>().EnsureDeleted();
            foreach (var entity in GetDbEntities(this))
            {
                var dropTableMethod = typeof(DataExtensions).GetMethods().FirstOrDefault(m => m.Name == "DropTable").MakeGenericMethod(entity);

                dropTableMethod.Invoke(this, new object[] { this, Type.Missing, Type.Missing, Type.Missing, Type.Missing,Type.Missing });
            }


            return isDbMigrate;
        }

        public  IServiceProvider GetServiceProvider()
        {

            if (_contextServices != null)
            {
                return _contextServices.InternalServiceProvider;
            }
            if (_initializing)
            {
                throw new InvalidOperationException("BlocksDbContext is initializing.");
            }

            try
            {
                var serviceScope = serviceProviderCache.GetOrAdd(this.dbContextOption)
                     .GetRequiredService<IServiceScopeFactory>()
                     .CreateScope();
                _serviceScope = serviceScope;
                var scopedServiceProvider = serviceScope.ServiceProvider;

                var contextServices = scopedServiceProvider.GetService<IDbContextServices>();

                contextServices.Initialize(scopedServiceProvider, dbContextOption, this);

                _contextServices = contextServices;
            }
            finally
            {
                _initializing = false;
            }

            return _contextServices.InternalServiceProvider;

        }

        public IDbConnection GetDbConnection()
        {
            return this.Connection;
        }
    }
}
