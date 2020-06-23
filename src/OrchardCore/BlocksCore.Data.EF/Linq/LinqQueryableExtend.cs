using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlocksCore.Data.Abstractions.Entities;
using BlocksCore.Data.EF.Paging;
using BlocksCore.Abstractions.UI.Paging;
namespace BlocksCore.Data.EF.Linq
{
    public static class LinqQueryableExtend
    {
        #region where

        public static IDbLinqQueryable<TEntity> Where<TEntity, T1>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, bool>> predicate) where TEntity : Entity
        {
            return dbLinqQueryable.Where((LambdaExpression)predicate);
        }
        public static IDbLinqQueryable<TEntity> Where<TEntity, T1, T2>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, bool>> predicate) where TEntity :Entity
        {
            return dbLinqQueryable.Where((LambdaExpression)predicate);
        }
        public static IDbLinqQueryable<TEntity> Where<TEntity, T1, T2, T3>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, bool>> predicate) where TEntity : Entity
        {
            return dbLinqQueryable.Where((LambdaExpression)predicate);
        }
        #endregion

        #region select


        public static List<TEntity> SelectToDynamicList<TEntity, T1, TOut>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, TOut>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TEntity>((LambdaExpression)selector);
        }
        public static List<TEntity> SelectToDynamicList<TEntity, T1, T2, TOut>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, TOut>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TEntity>((LambdaExpression)selector);
        }

        public static List<TEntity> SelectToDynamicList<TEntity, T1, T2, T3, TOut>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, TOut>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TEntity>((LambdaExpression)selector);
        }


        public static List<TDto> SelectToList<TEntity, T1, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, TDto>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TDto>((LambdaExpression)selector);
        }

        public static List<TDto> SelectToList<TEntity, T1, T2, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, TDto>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TDto>((LambdaExpression)selector);
        }

        public static List<TDto> SelectToList<TEntity, T1, T2, T3, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, TDto>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TDto>((LambdaExpression)selector);
        }

        public static List<TDto> SelectToList<TEntity, T1, T2, T3, T4, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, TDto>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TDto>((LambdaExpression)selector);
        }

        public static List<TDto> SelectToList<TEntity, T1, T2, T3, T4, T5, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, TDto>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TDto>((LambdaExpression)selector);
        }

        public static List<TDto> SelectToList<TEntity, T1, T2, T3, T4, T5, T6, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, TDto>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TDto>((LambdaExpression)selector);
        }
        public static List<TDto> SelectToList<TEntity, T1, T2, T3, T4, T5, T6, T7, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TDto>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TDto>((LambdaExpression)selector);
        }


        public static List<TDto> SelectToList<TEntity, T1, T2, T3, T4, T5, T6, T7, T8, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TDto>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TDto>((LambdaExpression)selector);
        }

        public static List<TDto> SelectToList<TEntity, T1, T2, T3, T4, T5, T6, T7, T8, T9, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TDto>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TDto>((LambdaExpression)selector);
        }

        public static List<TDto> SelectToList<TEntity, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TDto>> selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToList<TDto>((LambdaExpression)selector);
        }
        #endregion


        #region select async
        static Task<List<T>> SelectEntityToAsync<TEntity, T>(IDbLinqQueryable<TEntity> dbLinqQueryable, LambdaExpression selector) where TEntity : Entity
        {
            return dbLinqQueryable.SelectToListAsync<T>(selector);
        }


        public static Task<List<TEntity>> SelectToDynamicListAsync<TEntity, T1, TOut>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, TOut>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TEntity>(dbLinqQueryable, (LambdaExpression)selector);
        }
        public static Task<List<TEntity>> SelectToDynamicListAsync<TEntity, T1, T2, TOut>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, TOut>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TEntity>(dbLinqQueryable, (LambdaExpression)selector);
        }

        public static Task<List<TEntity>> SelectToDynamicListAsync<TEntity, T1, T2, T3, TOut>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, TOut>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TEntity>(dbLinqQueryable, (LambdaExpression)selector);
        }


        public static Task<List<TDto>> SelectToListAsync<TEntity, T1, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, TDto>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TDto>(dbLinqQueryable, (LambdaExpression)selector);
        }

        public static Task<List<TDto>> SelectToListAsync<TEntity, T1, T2, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, TDto>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TDto>(dbLinqQueryable, (LambdaExpression)selector);
        }

        public static Task<List<TDto>> SelectToListAsync<TEntity, T1, T2, T3, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, TDto>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TDto>(dbLinqQueryable, (LambdaExpression)selector);
        }

        public static Task<List<TDto>> SelectToListAsync<TEntity, T1, T2, T3, T4, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, TDto>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TDto>(dbLinqQueryable, (LambdaExpression)selector);
        }

        public static Task<List<TDto>> SelectToListAsync<TEntity, T1, T2, T3, T4, T5, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, TDto>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TDto>(dbLinqQueryable, (LambdaExpression)selector);
        }

        public static Task<List<TDto>> SelectToListAsync<TEntity, T1, T2, T3, T4, T5, T6, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, TDto>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TDto>(dbLinqQueryable, (LambdaExpression)selector);
        }
        public static Task<List<TDto>> SelectToListAsync<TEntity, T1, T2, T3, T4, T5, T6, T7, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TDto>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TDto>(dbLinqQueryable, (LambdaExpression)selector);
        }


        public static Task<List<TDto>> SelectToListAsync<TEntity, T1, T2, T3, T4, T5, T6, T7, T8, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TDto>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TDto>(dbLinqQueryable, (LambdaExpression)selector);
        }

        public static Task<List<TDto>> SelectToListAsync<TEntity, T1, T2, T3, T4, T5, T6, T7, T8, T9, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TDto>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TDto>(dbLinqQueryable, (LambdaExpression)selector);
        }

        public static Task<List<TDto>> SelectToListAsync<TEntity, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TDto>> selector) where TEntity : Entity
        {
            return SelectEntityToAsync<TEntity, TDto>(dbLinqQueryable, (LambdaExpression)selector);
        }
        #endregion
        #region paging
        public static PageList<TDto> Paging<TEntity, T1, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, TDto>> selector, Page page, bool distinct = false) where TEntity : Entity
        {
            return dbLinqQueryable.Paging<TDto>((LambdaExpression)selector, page, distinct);
        }

        public static PageList<TDto> Paging<TEntity, T1, T2, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, TDto>> selector, Page page, bool distinct = false) where TEntity : Entity
        {
            return dbLinqQueryable.Paging<TDto>((LambdaExpression)selector,page, distinct);
        }

        public static PageList<TDto> Paging<TEntity, T1, T2, T3, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, TDto>> selector, Page page, bool distinct = false) where TEntity : Entity
        {
            return dbLinqQueryable.Paging<TDto>((LambdaExpression)selector,page, distinct);
        }

        public static PageList<TDto> Paging<TEntity, T1, T2, T3, T4, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, TDto>> selector, Page page, bool distinct = false) where TEntity : Entity
        {
            return dbLinqQueryable.Paging<TDto>((LambdaExpression)selector,page, distinct);
        }

        public static PageList<TDto> Paging<TEntity, T1, T2, T3, T4, T5, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, TDto>> selector, Page page, bool distinct = false) where TEntity : Entity
        {
            return dbLinqQueryable.Paging<TDto>((LambdaExpression)selector,page, distinct);
        }

        public static PageList<TDto> Paging<TEntity, T1, T2, T3, T4, T5, T6, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, TDto>> selector, Page page, bool distinct = false) where TEntity : Entity
        {
            return dbLinqQueryable.Paging<TDto>((LambdaExpression)selector,page, distinct);
        }

        public static PageList<TDto> Paging<TEntity, T1, T2, T3, T4, T5, T6, T7, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TDto>> selector, Page page, bool distinct = false) where TEntity : Entity
        {
            return dbLinqQueryable.Paging<TDto>((LambdaExpression)selector,page, distinct);
        }

        public static PageList<TDto> Paging<TEntity, T1, T2, T3, T4, T5, T6, T7, T8, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TDto>> selector, Page page, bool distinct = false) where TEntity : Entity
        {
            return dbLinqQueryable.Paging<TDto>((LambdaExpression)selector,page, distinct);
        }

        public static PageList<TDto> Paging<TEntity, T1, T2, T3, T4, T5, T6, T7, T8, T9, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TDto>> selector, Page page, bool distinct = false) where TEntity : Entity
        {
            return dbLinqQueryable.Paging<TDto>((LambdaExpression)selector,page, distinct);
        }

        public static PageList<TDto> Paging<TEntity, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TDto>(this IDbLinqQueryable<TEntity> dbLinqQueryable, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TDto>> selector, Page page, bool distinct = false) where TEntity : Entity
        {
            return dbLinqQueryable.Paging<TDto>((LambdaExpression)selector,page, distinct);
        }


        #endregion
    }
}
