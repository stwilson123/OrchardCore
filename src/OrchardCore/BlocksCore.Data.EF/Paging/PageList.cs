using System;
using System.Collections.Generic;
using BlocksCore.Abstractions.Data.Paging;

namespace BlocksCore.Data.EF.Paging
{
    [Serializable]
    public class PageList<T> : IPageList<T>
    {
        public IPage PagerInfo { get; set; }

        public IList<T> Rows { get; set; }

    }
}
