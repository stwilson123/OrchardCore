using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlocksCore.Data.Abstractions.Entities;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Abstractions.UI.Paging;
namespace BlocksCore.Data.EF.Linq
{
    public interface IDbLinqQueryable<TEntity> : IDbLinqQueryable where TEntity : Entity
    {
        IQueryable iQuerable {  get; }

        IDbLinqQueryable<TEntity> Where(LambdaExpression predicate);

        List<DTO> SelectToList<DTO>(LambdaExpression selector);


        Task<List<DTO>> SelectToListAsync<DTO>(LambdaExpression selector);

        IDbLinqQueryable<TEntity> InnerJoin<TOuter, TInner, TKey>(
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector) where TKey : IComparable, IConvertible where TOuter : class where TInner : class;

        IDbLinqQueryable<TEntity> LeftJoin<TOuter, TInner, TKey>(
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector) where TKey : IComparable, IConvertible where TOuter : class where TInner : class;


        IDbLinqQueryable<TEntity> Take(int count);

        IDbLinqQueryable<TEntity> Skip(int count);

        IDbLinqQueryable<TEntity> OrderBy<TSource, TKey>(Expression<Func<TSource, TKey>> keySelector);

        IDbLinqQueryable<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> keySelector);

        IDbLinqQueryable<TEntity> OrderByDescending<TSource, TKey>(Expression<Func<TSource, TKey>> keySelector);
        IDbLinqQueryable<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector);


        IDbLinqQueryable<TEntity> ThenBy<TSource, TKey>(Expression<Func<TSource, TKey>> keySelector);


        IDbLinqQueryable<TEntity> ThenBy<TKey>(Expression<Func<TEntity, TKey>> keySelector);

        IDbLinqQueryable<TEntity> ThenByDescending<TSource, TKey>(Expression<Func<TSource, TKey>> keySelector);

        IDbLinqQueryable<TEntity> ThenByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector);

      //  PageList<DTO> Paging<DTO>(LambdaExpression selector,Page page);

        PageList<DTO> Paging<DTO>(LambdaExpression selector,Page page,bool distinct);

        long Count();
        string ToString();
    }

    public interface IDbLinqQueryable
    {

    }
}