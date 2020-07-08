using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlocksCore.Data.Abstractions.Entities;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Domain.Abstractions;
using BlocksCore.SyntacticAbstractions.Types.Collections;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace BlocksCore.Data.EF.Repository
{
    public class DBSqlRepositoryBase<TEntity, TPrimaryKey> : Abstractions.Repository.IRepository<TEntity, TPrimaryKey>,
        ISupportsExplicitLoading<TEntity, TPrimaryKey>,
        IRepositoryWithDbContext
        where TEntity : Entity<TPrimaryKey>
    {
        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
      //  public virtual TDbContext Context => _unitProvider.GetDbContext<TDbContext, TEntity>();

        public virtual DbContext Context
        {
            get
            {
                if (context == null)
                    context = _unitOfwork.GetOrCreateDataContext<TEntity>() as DbContext;
                return context;
            }
        }

        protected DbContext context;
        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>

        public virtual DbSet<TEntity> Table
        {
            get
            {
                Trace.TraceInformation($"Thread ID {Thread.CurrentThread.ManagedThreadId}, ContextObject {Context.GetHashCode()}");
                return Context.Set<TEntity>();
            }
        }


        private readonly IUnitOfWork _unitOfwork;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitProvider"></param>
        public DBSqlRepositoryBase(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfwork = unitOfWorkManager.Current;

            // _unitProvider = unitProvider;
            //Type type = typeof(TEntity);
            //var attr = type.GetSingleAttributeOfTypeOrBaseTypesOrNull<MultiTenancySideAttribute>();
            //if (attr != null)
            //{
            //    MultiTenancySide = attr.Side;
            //}
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.Table;
        }


        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            throw new NotSupportedException("This Method is not supported");
        }


        protected internal IQueryable<TEntity> GetAllCode()
        {
            return GetAllIncludingCode();
        }

        private IQueryable<TEntity> GetAllIncludingCode(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = Table.AsQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query.AsNoTracking();
        }

        public virtual TEntity Get(TPrimaryKey id)
        {
            var entity = FirstOrDefault(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = await FirstOrDefaultAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public List<TEntity> GetAllList()
        {
            return GetAllCode().ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(GetAllList());
        }

        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllCode().Where(predicate).ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        public TEntity FirstOrDefault(TPrimaryKey id)
        {
            return FirstOrDefault(CreateEqualityExpressionForId(id));
            // return GetAllCode().Where(CreateEqualityExpressionForId(id)).Skip(0).Take(1).FirstOrDefault();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllCode().FirstOrDefault(predicate);
            //return GetAllCode().Where(predicate).Skip(0).Take(1).ToArray().FirstOrDefault();
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return Task.FromResult(FirstOrDefault(id));
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }


        public virtual TEntity Load(TPrimaryKey id)
        {
            return Get(id);
        }


        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            //            return GetAllCode().Single(predicate);
            return GetAllCode().Where(predicate).Skip(0).Take(2).ToArray().Single();
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Single(predicate));
        }

        public virtual TEntity Insert(TEntity entity)
        {

            var EntityEntry = Table.Add(entity);
            Context.SaveChanges();
            return EntityEntry.Entity;
        }


        /// Inserts new entitites.
        /// </summary>
        /// <param name="entitites">Inserted entitites</param>
        public virtual IList<TEntity> Insert(IList<TEntity> entitites)
        {
            // Table.BulkInsert(entitites,o => o.);
            Table.AddRange(entitites);
            Context.SaveChanges();

            return entitites;
        }

        /// <summary>
        /// Inserts new entitites.
        /// </summary>
        /// <param name="entitites">Inserted entitites</param>
        public virtual Task<IList<TEntity>> InsertAsync(IList<TEntity> entity)
        {
            return Task.FromResult(Insert(entity));
        }


        public Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public TPrimaryKey InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity);

            if (entity.IsTransient())
            {
                Context.SaveChanges();
            }

            return entity.Id;
        }

        public virtual TEntity InsertOrUpdate(TEntity entity)
        {

            return entity.IsTransient()
                ? Insert(entity)
                : Update(entity);
        }

        public virtual async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return entity.IsTransient()
                ? await InsertAsync(entity)
                : await UpdateAsync(entity);
        }

        public async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);

            if (entity.IsTransient())
            {
                await Context.SaveChangesAsync();
            }

            return entity.Id;
        }


        public TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            entity = InsertOrUpdate(entity);

            if (entity.IsTransient())
            {
                Context.SaveChanges();
            }

            return entity.Id;
        }

        public async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            entity = await InsertOrUpdateAsync(entity);

            if (entity.IsTransient())
            {
                await Context.SaveChangesAsync();
            }

            return entity.Id;
        }

        public virtual TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            var entryEntity = Context.Entry(entity);

            entryEntity.State = EntityState.Modified;
            return entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Update(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(entity);
        }

        public virtual Int32 Update(Expression<Func<TEntity, bool>> wherePredicate,
            Expression<Func<TEntity, TEntity>> updateFactory)
        {
            return GetAllCode().Where(wherePredicate).Update(updateFactory);
        }

        public virtual Task<Int32> UpdateAsync(Expression<Func<TEntity, bool>> wherePredicate,
            Expression<Func<TEntity, TEntity>> updateFactory)
        {
            return GetAllCode().Where(wherePredicate).UpdateAsync(updateFactory);
        }

        public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
        {
            var entity = Get(id);
            updateAction(entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
        {
            var entity = await GetAsync(id);
            await updateAction(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.FromResult(0);
        }

        public void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            //Could not found the entity, do nothing.
        }

        public virtual Task DeleteAsync(TPrimaryKey id)
        {
            Delete(id);
            return Task.FromResult(0);
        }

        public long Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllCode().Where(predicate).Delete();
        }


        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Delete(predicate));
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllCode().Where(predicate).Count();
        }


        public long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllCode().Where(predicate).LongCount();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllCode().Any(predicate);
        }







        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = Context.ChangeTracker.Entries().FirstOrDefault(ent => entity == ent.Entity);

            if (entry != null)
            {
                return;
            }

            var entry1 = Context.Set<TEntity>().Local.FirstOrDefault(t => t.Id.ToString() == entity.Id.ToString());
            if (entry1 != null)
            {
                return;
            }

            Table.Attach(entity);
        }


        public T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            throw new NotSupportedException("This Method is not supported");
        }


        public virtual int Count()
        {
            return GetAllCode().Count();
        }

        public virtual Task<int> CountAsync()
        {
            return Task.FromResult(Count());
        }


        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }

        public virtual long LongCount()
        {
            return GetAllCode().LongCount();
        }

        public virtual Task<long> LongCountAsync()
        {
            return Task.FromResult(LongCount());
        }


        public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(LongCount(predicate));
        }

        public DbContext GetDbContext()
        {
            return Context;
        }

        public Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            return Context.Entry(entity).Collection(collectionExpression).LoadAsync(cancellationToken);
        }

        public Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            return Context.Entry(entity).Reference(propertyExpression).LoadAsync(cancellationToken);
        }

        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = Context.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, (ent.Entity as TEntity).Id)
                );

            return entry?.Entity as TEntity;
        }

        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}
