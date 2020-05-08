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
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Domain.Abstractions;
using BlocksCore.SyntacticAbstractions.Reflection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.EF
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// </summary>
    public abstract class BaseBlocksDbContext : DbContext ///, ITransientDependency, IShouldInitialize
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
        public IEnumerable<Type> EntityTypes { get; }


        // private readonly IEnumerable<IEntityConfiguration> _entityConfigurations;


        private ShellSettings _settingManager { get; set; }

        protected virtual bool isDbMigrate => false; 

        private Stopwatch sw = new Stopwatch();

        static IDictionary<Type, ValueTuple<bool, object>> entityConfigsDictionary = new ConcurrentDictionary<Type, (bool, object)>();
        /// <summary>
        /// Constructor.
        /// Uses <see cref="IAbpStartupConfiguration.DefaultNameOrConnectionString"/> as connection string.
        /// </summary>
        protected BaseBlocksDbContext(IEnumerable<Type> entityTypes, ShellSettings settingManager, ILogger log, DbContextOptions options) :base(options)
        {
            this.EntityTypes = entityTypes;
            _settingManager = settingManager;
            this.Logger = log;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
          
            //var dbType = _settingManager.GetSettingValueForApplication("DatabaseType");
            //var connectionString = ConfigurationManager.ConnectionStrings[nameOrConnectionString].ConnectionString;
            //switch (dbType)
            //{
            //    case "Sqlserver": optionsBuilder.UseSqlServer(connectionString: connectionString, sqlServerOptionsAction: b => b.UseRowNumberForPaging()); break;
            //    case "Oracle":
            //        optionsBuilder.UseOracle(connectionString: connectionString, oracleOptionsAction: (option) => option.UseOracleSQLCompatibility("11")); break;

            //        //                    optionsBuilder.UseOracle(connectionString: connectionString, oracleOptionsAction:b => b.UseRowNumberForPaging()); break;
            //}
        }



        //        /// <summary>
        //        /// Constructor.
        //        /// </summary>
        //        protected BaseBlocksDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
        //            : base(objectContext, dbContextOwnsObjectContext)
        //        {
        //            InitializeDbContext();
        //        }
        //
        //



        //private void RegisterToChanges()
        //{
        //    ((IObjectContextAdapter) this)
        //        .ObjectContext
        //        .ObjectStateManager
        //        .ObjectStateManagerChanged += ObjectStateManager_ObjectStateManagerChanged;
        //}

        //protected virtual void ObjectStateManager_ObjectStateManagerChanged(object sender,
        //    System.ComponentModel.CollectionChangeEventArgs e)
        //{
        //    var contextAdapter = (IObjectContextAdapter) this;
        //    if (e.Action != CollectionChangeAction.Add)
        //    {
        //        return;
        //    }

        //    var entry = contextAdapter.ObjectContext.ObjectStateManager.GetObjectStateEntry(e.Element);
        //    switch (entry.State)
        //    {
        //        case EntityState.Added:
        //            CheckAndSetId(entry.Entity);
        //            CheckAndSetMustHaveTenantIdProperty(entry.Entity);
        //            SetCreationAuditProperties(entry.Entity, GetAuditUserId());
        //            break;
        //        //case EntityState.Deleted: //It's not going here at all
        //        //    SetDeletionAuditProperties(entry.Entity, GetAuditUserId());
        //        //    break;
        //    }
        //}

        private void SetNullsForInjectedProperties()
        {

            //AbpSession = NullAbpSession.Instance;
            //EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
            //GuidGenerator = SequentialGuidGenerator.Instance;
            //EventBus = NullEventBus.Instance;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.Logger.LogDebug("Begin OnModelCreating");
            sw.Restart();
            modelBuilder.HasDefaultSchema(_settingManager[ConfigKey.Schema]);
            modelBuilder.RemovePluralizingTableNameConvention();
            // modelBuilder.Conventions.Remove<Microsoft.EntityFrameworkCore.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);


            //var entityTypes = this.typeFeatureExtensionsProvider.GetFeatureDenepenciesForDependency(this.GetType());
            var entityTypes = this.EntityTypes.Where(t => typeof(IEntity).IsAssignableFrom(t));
            var queryEntityTypes = this.EntityTypes.Where(t => typeof(IQueryEntity).IsAssignableFrom(t));

            foreach (var queryEntity in queryEntityTypes)
            {
                modelBuilder.Entity(queryEntity).HasNoKey();
            }

            foreach (var entity in entityTypes)
            {
                if (!isDbMigrate)
                {
                    modelBuilder.Entity(entity);
                }
                else
                {
                    modelBuilder.Entity(entity,entityTypeBuilder => {

                        foreach (var entityProperty in entity.GetProperties().Where(p => !TypeHelper.IsPrimitiveExtendedIncludingNullable(p.PropertyType)))
                        {
                            entityTypeBuilder.Ignore(entityProperty.Name);
                        }
                    });
                }

            }
            var ApplyConfigurationMethod = typeof(ModelBuilder).GetMethods().Single<MethodInfo>((e =>
            {
                if (e.Name == "ApplyConfiguration" && e.ContainsGenericParameters)
                    return e.GetParameters().SingleOrDefault()?.ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>);
                return false;
            }));
            //MethodInfo methodInfo2 = ((IEnumerable<MethodInfo>)typeof(ModelBuilder).GetMethods()).Single<MethodInfo>((Func<MethodInfo, bool>)(e =>
            //{
            //    if (e.Name == "ApplyConfiguration" && e.ContainsGenericParameters)
            //        return ((IEnumerable<ParameterInfo>)e.GetParameters()).SingleOrDefault<ParameterInfo>()?.ParameterType.GetGenericTypeDefinition() == typeof(IQueryTypeConfiguration<>);
            //    return false;
            //}));


            foreach (var entityTypeConfiguration in this.EntityTypes)
            {
                foreach (var entityTypeInterfaceType in entityTypeConfiguration.GetInterfaces().Where(InterfaceType => InterfaceType.IsGenericType && InterfaceType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                {
                    ApplyConfigurationMethod.MakeGenericMethod(entityTypeInterfaceType.GenericTypeArguments[0]).Invoke(modelBuilder, new object[1]
                     {
                          Activator.CreateInstance(entityTypeConfiguration)
                     });
                }
            }

            //var registerAssembly = System.AppDomain.CurrentDomain.GetAssemblies().Where(t =>
            //   _entityConfigurations.Any(config => string.Equals(t.GetName().Name, config.EntityModule, StringComparison.CurrentCultureIgnoreCase)));



            //foreach (var assembly in registerAssembly.DistinctBy(t => t.FullName))
            //{
            //    //
            //    //                if (type.GetGenericTypeDefinition() == typeof (IEntityTypeConfiguration<>))
            //    //                    methodInfo1.MakeGenericMethod(type.GenericTypeArguments[0]).Invoke((object) this, new object[1]
            //    //                    {
            //    //                        Activator.CreateInstance((Type) constructibleType)
            //    //                    });
            //    foreach (var queryType in assembly.GetTypes().Where(t => typeof(IQueryEntity).IsAssignableFrom(t)))
            //    {
            //        modelBuilder.Query(queryType);
            //    }

            //    Stopwatch swAss = Stopwatch.StartNew();
            //    modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            //    Logger.Logger(new LogModel() { LogSeverity = LogSeverity.Debug, Message = $"ApplyConfigurationsFromAssembly cost time {swAss.ElapsedMilliseconds}ms" });

            //    //modelBuilder.Configurations.AddFromAssembly(assembly);
            //}
            sw.Stop();
            this.Logger.LogDebug($"End OnModelCreating cost time {sw.ElapsedMilliseconds}ms");


            //TODO global filter extensions
            //modelBuilder.Filter(AbpDataFilters.SoftDelete, (ISoftDelete d) => d.IsDeleted, false);
            //modelBuilder.Filter(AbpDataFilters.MustHaveTenant,
            //    (IMustHaveTenant t, int tenantId) => t.TenantId == tenantId || (int?) t.TenantId == null,
            //    0); //While "(int?)t.TenantId == null" seems wrong, it's needed. See https://github.com/jcachat/EntityFramework.DynamicFilters/issues/62#issuecomment-208198058
            //modelBuilder.Filter(AbpDataFilters.MayHaveTenant,
            //    (IMayHaveTenant t, int? tenantId) => t.TenantId == tenantId, 0);
        }

        public override int SaveChanges()
        {
            try
            {

              //  var changedEntities = ApplyAbpConcepts();
                var result = base.SaveChanges();
               // EntityChangeEventHelper.TriggerEvents(changedEntities);
                return result;
            }
            catch (DbUpdateException ex)
            {
                LogDbEntityValidationException(ex);
                throw;
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
               // var changeReport = ApplyAbpConcepts();
                var result = await base.SaveChangesAsync(cancellationToken);
               // await EntityChangeEventHelper.TriggerEventsAsync(changeReport);
                return result;
            }
            catch (DbUpdateException ex)
            {
                LogDbEntityValidationException(ex);
                throw;
            }
        }

        //protected virtual EntityChangeReport ApplyAbpConcepts()
        //{
        //    var changeReport = new EntityChangeReport();

        //    var userId = GetAuditUserId();

        //    foreach (var entry in ChangeTracker.Entries().ToList())
        //    {
        //        ApplyAbpConcepts(entry, userId, changeReport);
        //    }

        //    return changeReport;
        //}

        //protected virtual void ApplyAbpConcepts(EntityEntry entry, long? userId, EntityChangeReport changeReport)
        //{
        //    switch (entry.State)
        //    {
        //        case EntityState.Added:
        //            ApplyAbpConceptsForAddedEntity(entry, userId, changeReport);
        //            break;
        //        case EntityState.Modified:
        //            ApplyAbpConceptsForModifiedEntity(entry, userId, changeReport);
        //            break;
        //        case EntityState.Deleted:
        //            ApplyAbpConceptsForDeletedEntity(entry, userId, changeReport);
        //            break;
        //    }

        //    AddDomainEvents(changeReport.DomainEvents, entry.Entity);
        //}

        //protected virtual void ApplyAbpConceptsForAddedEntity(EntityEntry entry, long? userId, EntityChangeReport changeReport)
        //{
        //    CheckAndSetId(entry.Entity);
        //    CheckAndSetMustHaveTenantIdProperty(entry.Entity);
        //    CheckAndSetMayHaveTenantIdProperty(entry.Entity);
        //    SetCreationAuditProperties(entry.Entity, userId);
        //    changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Created));
        //}

        //protected virtual void ApplyAbpConceptsForModifiedEntity(EntityEntry entry, long? userId, EntityChangeReport changeReport)
        //{
        //    SetModificationAuditProperties(entry.Entity, userId);

        //    if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
        //    {
        //        SetDeletionAuditProperties(entry.Entity, userId);
        //        changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
        //    }
        //    else
        //    {
        //        changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Updated));
        //    }
        //}

        //protected virtual void ApplyAbpConceptsForDeletedEntity(EntityEntry entry, long? userId, EntityChangeReport changeReport)
        //{
        //    CancelDeletionForSoftDelete(entry);
        //    SetDeletionAuditProperties(entry.Entity, userId);
        //    changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
        //}

        //protected virtual void AddDomainEvents(List<DomainEventEntry> domainEvents, object entityAsObj)
        //{
        //    var generatesDomainEventsEntity = entityAsObj as IGeneratesDomainEvents;
        //    if (generatesDomainEventsEntity == null)
        //    {
        //        return;
        //    }

        //    if (generatesDomainEventsEntity.DomainEvents.IsNullOrEmpty())
        //    {
        //        return;
        //    }

        //    domainEvents.AddRange(
        //        generatesDomainEventsEntity.DomainEvents.Select(
        //            eventData => new DomainEventEntry(entityAsObj, eventData)));
        //    generatesDomainEventsEntity.DomainEvents.Clear();
        //}

        protected virtual void CheckAndSetId(object entityAsObj)
        {
            //Set GUID Ids  TODO
            //var entity = entityAsObj as IEntity<string>;
            //if (entity != null && string.IsNullOrEmpty(entity.Id))
            //{

            //    var entityType = entityAsObj.GetType();//ObjectContext.GetObjectType(entityAsObj.GetType());
            //    var idProperty = entityType.GetProperty("Id");
            //    var dbGeneratedAttr =
            //        ReflectionHelper.GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(idProperty);
            //    if (dbGeneratedAttr == null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
            //    {
            //        entity.Id = GuidGenerator.Create().ToString();
            //    }
            //}
        }

        //protected virtual void CheckAndSetMustHaveTenantIdProperty(object entityAsObj)
        //{
        //    if (SuppressAutoSetTenantId)
        //    {
        //        return;
        //    }

        //    //Only set IMustHaveTenant entities
        //    if (!(entityAsObj is IMustHaveTenant))
        //    {
        //        return;
        //    }

        //    var entity = entityAsObj.As<IMustHaveTenant>();

        //    //Don't set if it's already set
        //    if (entity.TenantId != 0)
        //    {
        //        return;
        //    }

        //    var currentTenantId = GetCurrentTenantIdOrNull();

        //    if (currentTenantId != null)
        //    {
        //        entity.TenantId = currentTenantId.Value;
        //    }
        //    else
        //    {
        //        throw new BlocksDataException("Can not set TenantId to 0 for IMustHaveTenant entities!");
        //    }
        //}

        //protected virtual void CheckAndSetMayHaveTenantIdProperty(object entityAsObj)
        //{
        //    if (SuppressAutoSetTenantId)
        //    {
        //        return;
        //    }

        //    //Only set IMayHaveTenant entities
        //    if (!(entityAsObj is IMayHaveTenant))
        //    {
        //        return;
        //    }

        //    var entity = entityAsObj.As<IMayHaveTenant>();

        //    //Don't set if it's already set
        //    if (entity.TenantId != null)
        //    {
        //        return;
        //    }

        //    //Only works for single tenant applications
        //    if (MultiTenancyConfig?.IsEnabled ?? false)
        //    {
        //        return;
        //    }

        //    //Don't set if MayHaveTenant filter is disabled
        //    //if (!this.IsFilterEnabled(AbpDataFilters.MayHaveTenant))
        //    //{
        //    //    return;
        //    //}

        //    entity.TenantId = GetCurrentTenantIdOrNull();
        //}

        //protected virtual void SetCreationAuditProperties(object entityAsObj, long? userId)
        //{
        //    EntityAuditingHelper.SetCreationAuditProperties(MultiTenancyConfig, entityAsObj, AbpSession.TenantId,
        //        userId);
        //}

        //protected virtual void SetModificationAuditProperties(object entityAsObj, long? userId)
        //{
        //    EntityAuditingHelper.SetModificationAuditProperties(MultiTenancyConfig, entityAsObj, AbpSession.TenantId,
        //        userId);
        //}

        //protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        //{
        //    if (!(entry.Entity is ISoftDelete))
        //    {
        //        return;
        //    }

        //    var softDeleteEntry = entry;
        //    softDeleteEntry.Reload();
        //    softDeleteEntry.State = EntityState.Modified;
        //    ((ISoftDelete)softDeleteEntry.Entity).IsDeleted = true;
        //}

        //protected virtual void SetDeletionAuditProperties(object entityAsObj, long? userId)
        //{
        //    if (entityAsObj is IHasDeletionTime)
        //    {
        //        var entity = entityAsObj.As<IHasDeletionTime>();

        //        if (entity.DeletionTime == null)
        //        {
        //            entity.DeletionTime = Clock.Now;
        //        }
        //    }

        //    if (entityAsObj is IDeletionAudited)
        //    {
        //        var entity = entityAsObj.As<IDeletionAudited>();

        //        if (entity.DeleterUserId != null)
        //        {
        //            return;
        //        }

        //        if (userId == null)
        //        {
        //            entity.DeleterUserId = null;
        //            return;
        //        }

        //        //Special check for multi-tenant entities
        //        if (entity is IMayHaveTenant || entity is IMustHaveTenant)
        //        {
        //            //Sets LastModifierUserId only if current user is in same tenant/host with the given entity
        //            if ((entity is IMayHaveTenant && entity.As<IMayHaveTenant>().TenantId == AbpSession.TenantId) ||
        //                (entity is IMustHaveTenant && entity.As<IMustHaveTenant>().TenantId == AbpSession.TenantId))
        //            {
        //                entity.DeleterUserId = userId;
        //            }
        //            else
        //            {
        //                entity.DeleterUserId = null;
        //            }
        //        }
        //        else
        //        {
        //            entity.DeleterUserId = userId;
        //        }
        //    }
        //}

        protected virtual void LogDbEntityValidationException(DbUpdateException exception)
        {
            Logger.LogError("There are some validation errors while saving changes in EntityFramework:");

            foreach (var ve in exception.Entries.SelectMany(eve => eve.Properties))
            {
                Logger.LogError(" - " + ve.OriginalValue + ": " + ve.CurrentValue);
            }
        }

        //protected virtual long? GetAuditUserId()
        //{
        //    if (AbpSession.UserId.HasValue &&
        //        CurrentUnitOfWorkProvider != null &&
        //        CurrentUnitOfWorkProvider.Current != null &&
        //        CurrentUnitOfWorkProvider.Current.GetTenantId() == AbpSession.TenantId)
        //    {
        //        return AbpSession.UserId;
        //    }

        //    return null;
        //}

        //protected virtual int? GetCurrentTenantIdOrNull()
        //{
        //    if (CurrentUnitOfWorkProvider?.Current != null)
        //    {
        //        return CurrentUnitOfWorkProvider.Current.GetTenantId();
        //    }

        //    return AbpSession.TenantId;
        //}
    }
}
