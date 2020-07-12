using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
using BlocksCore.Domain.Abstractions;
using BlocksCore.SyntacticAbstractions.Reflection.Extensions;
using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.Linq2DB
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// </summary>
    public abstract class BaseBlocksDbContext : DataConnection ///, ITransientDependency, IShouldInitialize
    {
        /// <summary>
        /// Used to get current session values.
        /// </summary>
       // public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// Used to trigger entity change events.
        /// </summary>
     //   public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

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

        /// <summary>
        /// Can be used to suppress automatically setting TenantId on SaveChanges.
        /// Default: false.
        /// </summary>
        public bool SuppressAutoSetTenantId { get; set; }


        private ShellSettings _settingManager { get; set; }

        protected virtual bool isDbMigrate { get; set; } = false;

        protected readonly IEnumerable<Type> entityTypes;

        public IQueryable Get<TEntity>() where TEntity : class
        {
            return this.GetTable<TEntity>();
        }
        /// </summary>
        protected BaseBlocksDbContext(ShellSettings settingManager, ILogger log, string connectionString) : base(connectionString)
        {
            _settingManager = settingManager;
            this.Logger = log;
        }


        /// <summary>
        /// Constructor.
        /// Uses <see cref="IAbpStartupConfiguration.DefaultNameOrConnectionString"/> as connection string.
        /// </summary>
        protected BaseBlocksDbContext(IEnumerable<Type> entityTypes, ShellSettings settingManager, ILogger log, LinqToDbConnectionOptions options) : base(options)
        {
            this.entityTypes = entityTypes;
            _settingManager = settingManager;
            this.Logger = log;
        }



        public bool EnsureCreated()
        {
            if (!isDbMigrate)
                return false;
           
            foreach (var entity in GetDbEntities(this))
            {
                var createTableMethod = typeof(DataExtensions).GetMethod("CreateTable").MakeGenericMethod(entity);
                createTableMethod.Invoke(this, new object[] { this, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing });

            }  
            
            
            return isDbMigrate;
        }


        private IEnumerable<Type> GetDbEntities(BaseBlocksDbContext context)
        {

            var dbEntitits = entityTypes.Where(p => !p.IsAbstract && !p.IsInterface && typeof(IEntity).IsAssignableFrom(p));
            return dbEntitits;
        }

        public bool EnsureDeleted()
        {
            if (!isDbMigrate)
                return false;
            foreach (var entity in GetDbEntities(this))
            {
                var dropTableMethod = typeof(DataExtensions).GetMethods().FirstOrDefault(m => m.Name == "DropTable").MakeGenericMethod(entity);

                dropTableMethod.Invoke(this, new object[] { this, Type.Missing, Type.Missing, Type.Missing, Type.Missing,Type.Missing });
            }


            return isDbMigrate;
        }
    }
}
