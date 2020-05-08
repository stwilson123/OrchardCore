using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Abstractions.Data.Paging
{
    public interface IPageList<T>
    {
        public IPage PagerInfo { get; set; }

        public IList<T> Rows { get; set; }
    }
}
