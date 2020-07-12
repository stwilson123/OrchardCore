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
using BlocksCore.Data.Linq2DB.DBContext;
using BlocksCore.Domain.Abstractions;
using BlocksCore.SyntacticAbstractions.Types.Collections;
using LinqToDB;
using LinqToDB.Data;

namespace BlocksCore.Data.Linq2DB.Repository
{
    public class DBSqlRepositoryBase<TEntity, TPrimaryKey> : Abstractions.Repository.IRepository<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
    {
        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
      //  public virtual TDbContext Context => _unitProvider.GetDbContext<TDbContext, TEntity>();

        public virtual BlocksDbContext Context
        {
            get
            {
                if (_context == null)
                    _context = _unitOfwork.GetOrCreateDataContext<TEntity>() as BlocksDbContext;
                return _context;
            }
        }

        protected BlocksDbContext _context;
        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>

        public virtual ITable<TEntity> Table
        {
            get
            {
                Trace.TraceInformation($"Thread ID {Thread.CurrentThread.ManagedThreadId}, ContextObject {Context.GetHashCode()}");

                return Context.GetTable<TEntity>();
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
            return Table;

        }

        private IQueryable<TEntity> GetAllIncludingCode(params Expression<Func<TEntity, object>>[] propertySelectors)
        {

            throw new NotSupportedException("This Method is not supported");
            //var query = Table.AsQueryable();

            //if (!propertySelectors.IsNullOrEmpty())
            //{
            //    foreach (var propertySelector in propertySelectors)
            //    {
            //        query = query.Include(propertySelector);
            //    }
            //}

            //return query.AsNoTracking();
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

            return (Context.Insert(entity) > 0) ? entity : default(TEntity);
        }


        /// Inserts new entitites.
        /// </summary>
        /// <param name="entitites">Inserted entitites</param>
        public virtual IList<TEntity> Insert(IList<TEntity> entitites)
        {
            // Table.BulkInsert(entitites,o => o.);
            return (Context.BulkCopy(entitites).RowsCopied > 0) ? entitites : default(IList<TEntity>);
        }

        /// <summary>
        /// Inserts new entitites.
        /// </summary>
        /// <param name="entitites">Inserted entitites</param>
        public virtual Task<IList<TEntity>> InsertAsync(IList<TEntity> entities)
        {
            var result = (Context.BulkCopy(entities).RowsCopied > 0) ? entities : default(IList<TEntity>);
            return Task.FromResult(result);
        }


        public async virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            return (await Context.InsertAsync(entity) > 0) ? entity : default(TEntity);
        }

        public TPrimaryKey InsertAndGetId(TEntity entity)
        {

            return (TPrimaryKey)(Context.InsertWithIdentity(entity));
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
            
            return entity.Id;
        }


        public TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            entity = InsertOrUpdate(entity);

            return entity.Id;
        }

        public async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            entity = await InsertOrUpdateAsync(entity);
            return entity.Id;
        }

        public virtual TEntity Update(TEntity entity)
        {
            return Context.Update(entity) > 0 ? entity : default(TEntity);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return (await Context.UpdateAsync(entity)) > 0 ? entity : default(TEntity); 
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

        public virtual int Delete(TEntity entity)
        {
            return Context.Delete(entity);
        }

        public virtual Task<int> DeleteAsync(TEntity entity)
        {
            return Context.DeleteAsync(entity);
        }

        public virtual int Delete(TPrimaryKey id)
        {
            
            return GetAllCode().Where(CreateEqualityExpressionForId(id)).Delete();
        }

        public virtual Task<int> DeleteAsync(TPrimaryKey id)
        {
            return GetAllCode().Where(CreateEqualityExpressionForId(id)).DeleteAsync();

        }

        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllCode().Where(predicate).Delete();
        }


        public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllCode().Where(predicate).DeleteAsync();

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

        public BlocksDbContext GetDbContext()
        {
            return Context;
        }

        //public Task EnsureCollectionLoadedAsync<TProperty>(
        //    TEntity entity,
        //    Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
        //    CancellationToken cancellationToken)
        //    where TProperty : class
        //{
        //    return Context.Entry(entity).Collection(collectionExpression).LoadAsync(cancellationToken);
        //}

        //public Task EnsurePropertyLoadedAsync<TProperty>(
        //    TEntity entity,
        //    Expression<Func<TEntity, TProperty>> propertyExpression,
        //    CancellationToken cancellationToken)
        //    where TProperty : class
        //{
        //    return Context.Entry(entity).Reference(propertyExpression).LoadAsync(cancellationToken);
        //}


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
