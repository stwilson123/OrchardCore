using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using BlocksCore.Abstractions.Data.Paging;
using BlocksCore.Data.Abstractions.Entities;
using BlocksCore.Data.EF.Paging;
using Microsoft.EntityFrameworkCore;
using BlocksCore.Abstractions.UI.Paging;
namespace BlocksCore.Data.EF.DBContext
{
    public static class DbContextExtensions
    {
        public static List<TElement> SqlQuery<TElement>([NotNull]this DbContext context ,string sql, params object[] paramters)
           where TElement : class, IQueryEntity
        {
            return context.Set<TElement>().FromSqlRaw(sql, paramters).ToList();
        }

        public static int ExecuteSqlCommand([NotNullAttribute]this DbContext context,string sql, params object[] paramters)
        {
            return context.Database.ExecuteSqlRaw(sql, paramters);
        }

        public static IPageList<TElement> SqlQueryPaging<TElement>(this DbContext context ,IPage page, string sql, params object[] paramters)  where TElement : class, IQueryEntity
        {

            var sqlQuery = context.Set<TElement>()
                .FromSqlRaw(sql, paramters);


            if (page.filters != null && page.filters.rules != null && page.filters.rules.Any())
            {
                var whereString = PageDynamicSearch.getStringForGroup(page.filters, null);
                sqlQuery = System.Linq.Dynamic.Core.DynamicQueryableExtensions.Where(sqlQuery, whereString);
            }
            if (!string.IsNullOrEmpty(page.OrderBy()))
                sqlQuery = System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(sqlQuery, page.OrderBy());

            if (!page.pageSize.HasValue || page.pageSize.Value <= 0)
            {
                var rows = sqlQuery.ToList();
                var pagelist = new PageList<TElement>()
                {
                    Rows = rows,
                    PagerInfo = new Page()
                    {
                        page = page.page,
                        pageSize = page.pageSize,
                        records = rows.Count,
                        sortColumn = page.sortColumn,
                        sortOrder = page.sortOrder
                    }
                };

                return pagelist;
            }
            else
            {

                var pageResult = System.Linq.Dynamic.Core.DynamicQueryableExtensions.PageResult<TElement>(sqlQuery, page.page, page.pageSize.Value);
                var pagelist = new PageList<TElement>()
                {
                    Rows = pageResult.Queryable.ToList(),
                    PagerInfo = new Page()
                    {
                        page = pageResult.CurrentPage,
                        pageSize = pageResult.PageSize,
                        records = pageResult.RowCount,
                        sortColumn = page.sortColumn,
                        sortOrder = page.sortOrder
                    }
                };
                return pagelist;
            }

        }
    }
}
