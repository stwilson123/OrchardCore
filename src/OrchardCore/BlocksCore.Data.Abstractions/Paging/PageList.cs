using System;
using System.Collections.Generic;
using BlocksCore.Abstractions.Data.Paging;
using BlocksCore.Abstractions.Datatransfer;

namespace BlocksCore.Data.Abstractions.Paging
{
    [Serializable]
    public class PageList<T> : IPageList<T>
    {
        [DataTransfer("pagerInfo")]
        public IPage PagerInfo { get; set; }

        [DataTransfer("rows")]
        public IList<T> Rows { get; set; }

    }
}
