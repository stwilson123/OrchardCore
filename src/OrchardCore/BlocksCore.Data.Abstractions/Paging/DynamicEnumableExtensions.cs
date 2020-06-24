using System;
using System.Collections.Generic;
using System.Linq;
using BlocksCore.SyntacticAbstractions.Types;

namespace BlocksCore.Data.Abstractions.Paging
{
    public static class DynamicEnumableExtensions
    {
        public static PagedResult<TSource> PageResult<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            Check.NotNull<IEnumerable<TSource>>(source, nameof(source));
            Check.Condition<int>(page, (Predicate<int>)(p => p > 0), nameof(page));
            Check.Condition<int>(pageSize, (Predicate<int>)(ps => ps > 0), nameof(pageSize));
            var pagedResult = new PagedResult<TSource>()
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = source.Count()
            };
            pagedResult.PageCount = (int)Math.Ceiling((double)pagedResult.RowCount / (double)pageSize);
            pagedResult.Queryable = source.Page(page, pageSize);
            return pagedResult;
        }

        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            Check.NotNull<IEnumerable<TSource>>(source, nameof(source));
            Check.Condition<int>(page, (Predicate<int>)(p => p > 0), nameof(page));
            Check.Condition<int>(pageSize, (Predicate<int>)(ps => ps > 0), nameof(pageSize));
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }



    }
}
