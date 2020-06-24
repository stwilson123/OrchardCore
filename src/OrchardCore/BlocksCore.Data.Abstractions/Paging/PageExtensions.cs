using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions.Data.Paging;
using BlocksCore.SyntacticAbstractions.Types;

namespace BlocksCore.Data.Abstractions.Paging
{
    public static  class PageExtensions
    {
        public static string OrderBy(this IPage page)
        {
            Check.NotNull(page, nameof(page));
            if (string.IsNullOrEmpty(page.sortColumn) || string.IsNullOrEmpty(page.sortOrder))
                return "";
            return $"{page.sortColumn} {page.sortOrder}";
        }
    }
}
