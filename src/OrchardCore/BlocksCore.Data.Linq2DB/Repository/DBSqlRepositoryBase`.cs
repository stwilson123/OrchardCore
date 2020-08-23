using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlocksCore.Abstractions.Data.Paging;
using BlocksCore.Abstractions.Security;
using BlocksCore.Data.Abstractions.Entities;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Data.Linq;
using BlocksCore.Data.Linq2DB.DBContext;
using BlocksCore.Domain.Abstractions;
using BlocksCore.SyntacticAbstractions.Types.Collections;
using LinqToDB;
using OrchardCore.Modules;

namespace BlocksCore.Data.Linq2DB.Repository
{
    public class DBSqlRepositoryBase<TEntity> : DBSqlRepositoryBase<TEntity, string>
        where TEntity : Entity
    {
        protected DbSetContext<BlocksDbContext> Tables
        {
            get
            {
                if (tables == null)
                    tables = new DbSetContext<BlocksDbContext>(this.Context);
                return tables;
            }
        }

        DbSetContext<BlocksDbContext> tables;

        public IUserContext UserContext { set; get; }
        public IClock Clock { set; get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitProvider"></param>
        public DBSqlRepositoryBase(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        public IDbLinqQueryable<TEntity> GetContextTable()
        {
            return new DefaultLinqQueryable<TEntity>(this.Context.GetTable<TEntity>(), Context) { };
        }

        //public IDbLinqQueryable<TEntity> GetContextTableIncluding(
        //    params Expression<Func<TEntity, object>>[] propertySelectors)
        //{
        //    var query = Table.AsQueryable();

        //    if (!propertySelectors.IsNullOrEmpty())
        //    {
        //        foreach (var propertySelector in propertySelectors)
        //        {
        //            query = query.Include(propertySelector);
        //        }
        //    }

        //    return new DefaultLinqQueryable<TEntity>(query, Context) { };
        //}


        public override TEntity Insert(TEntity entity)
        {
            var EntityObj = (Entity) entity;
            EntityObj.CREATER = string.IsNullOrEmpty(EntityObj.CREATER)
                ? UserContext.GetCurrentUser()?.UserId
                : EntityObj.CREATER;
            EntityObj.UPDATER = string.IsNullOrEmpty(EntityObj.UPDATER)
                ? UserContext.GetCurrentUser()?.UserId
                : EntityObj.UPDATER;
            EntityObj.CREATEDATE = Clock.UtcNow;
            EntityObj.UPDATEDATE = Clock.UtcNow;

            return base.Insert(entity);
        }

        public override TEntity Update(TEntity entity)
        {
            var EntityObj = (Entity) entity;

            EntityObj.UPDATER = string.IsNullOrEmpty(EntityObj.UPDATER)
                ? UserContext.GetCurrentUser()?.UserId
                : EntityObj.UPDATER;
            EntityObj.UPDATEDATE = Clock.UtcNow;

            return base.Update(entity);
        }

        public override IList<TEntity> Insert(IList<TEntity> entities)
        {
            if (entities == null)
                return entities;
            foreach (var entity in entities)
            {
                var EntityObj = (Entity) entity;
                EntityObj.CREATER = string.IsNullOrEmpty(EntityObj.CREATER)
                    ? UserContext.GetCurrentUser()?.UserId
                    : EntityObj.CREATER;
                EntityObj.UPDATER = string.IsNullOrEmpty(EntityObj.UPDATER)
                    ? UserContext.GetCurrentUser()?.UserId
                    : EntityObj.UPDATER;
                EntityObj.CREATEDATE = Clock.UtcNow;
                EntityObj.UPDATEDATE = Clock.UtcNow;
            }


            return base.Insert(entities);
        }

        public override int Update(Expression<Func<TEntity, bool>> wherePredicate,
            Expression<Func<TEntity, TEntity>> updateFactory)
        {
            var updateExpressionBody = updateFactory.Body;

            while (updateExpressionBody.NodeType == ExpressionType.Convert ||
                   updateExpressionBody.NodeType == ExpressionType.ConvertChecked)
            {
                updateExpressionBody = ((UnaryExpression) updateExpressionBody).Operand;
            }

            var entityType = typeof(TEntity);

            // ENSURE: new T() { MemberInitExpression }
            var memberInitExpression = updateExpressionBody as MemberInitExpression;
            if (memberInitExpression == null)
            {
                throw new Exception("Invalid Cast. The update expression must be of type MemberInitExpression.");
            }

            var MemberBindings = new List<MemberBinding>();
            MemberBindings.AddRange(memberInitExpression.Bindings);
            if (!MemberBindings.Any(t => t.Member.Name == "UPDATER"))
            {
                MemberBindings.Add(Expression.Bind(typeof(TEntity).GetMember("UPDATER")[0],
                    Expression.Constant(UserContext.GetCurrentUser().UserId)));
            }

            if (!MemberBindings.Any(t => t.Member.Name == "UPDATEDATE"))
            {
                MemberBindings.Add(Expression.Bind(typeof(TEntity).GetMember("UPDATEDATE")[0],
                    Expression.Constant(Clock.UtcNow)));
            }

            if (!MemberBindings.Any(t => t.Member.Name == "DATAVERSION") && updateFactory.Parameters.Any())
            {
                var lambdaParam = updateFactory.Parameters.FirstOrDefault();
                MemberBindings.Add(Expression.Bind(typeof(TEntity).GetMember("DATAVERSION")[0],
                    Expression.Add(Expression.PropertyOrField(lambdaParam, "DATAVERSION"),
                        Expression.Constant((long) 1))));
            }

            var updateMemberInit = memberInitExpression.Update(memberInitExpression.NewExpression, MemberBindings);

            Expression<Func<TEntity, TEntity>> UpdateExpression = Expression.Lambda<Func<TEntity, TEntity>>(
                updateMemberInit, updateFactory.Parameters
            );

            return GetAllCode().Where(wherePredicate).Update(updateFactory);
            //return GetAllCode().Where(wherePredicate).Update(UpdateExpression);
        }

        public IPageList<TElement> SqlQueryPaging<TElement>(IPage page,string sql, params object[] paramters) where TElement : class, IQueryEntity
        {
            return this.Context.SqlQueryPaging<TElement>(page, sql, paramters);
        }
        public IList<TElement> SqlQuery<TElement>(string sql, params object[] paramters) where TElement : class, IQueryEntity
        {
            return this.Context.SqlQuery<TElement>(sql, paramters);
        }

        public int ExecuteSqlCommand(string sql, params object[] paramters)
        {
           return  this.Context.ExecuteSqlCommand(sql, paramters);
        }

        public int ExecuteSqlCommand(string sql, object paramter)
        {
            return this.Context.ExecuteSqlCommand(sql, paramter);
        }
    }


    public class DbSetContext<TDbContext> where TDbContext : BlocksDbContext
    {
        private readonly TDbContext _context;
        private ConcurrentDictionary<Type, object> dbSetCache;

        public DbSetContext(TDbContext context)
        {
            _context = context;
            this.dbSetCache = new ConcurrentDictionary<Type, object>();
        }

        public ITable<TEntity> GetTable<TEntity>() where TEntity : Entity
        {
            return (ITable<TEntity>) dbSetCache.GetOrAdd(typeof(TEntity), type =>
                _context.GetTable<TEntity>()
            );
        }
    }
}