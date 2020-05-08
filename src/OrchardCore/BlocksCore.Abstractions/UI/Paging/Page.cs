using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions.Data.Paging;

namespace BlocksCore.Abstractions.UI.Paging
{
    public abstract class PageBase : IPage
    {


        public int? pageSize { get; set; }
        public int page { get; set; }
        public string sortColumn { get; set; }
        public string sortOrder { get; set; }

        public int total
        {
            get
            {

                if (!pageSize.HasValue || pageSize <= 0) return 0;
                var pageSizeTmp = pageSize.Value;
                return records % pageSizeTmp == 0 ? records / pageSizeTmp : records / pageSizeTmp + 1;
            }
        }

        public int records { get; set; }

        public int StartIndex
        {
            get
            {
                if (!pageSize.HasValue || pageSize <= 0) return 1;

                // int size = this.pageSize * this.page;
                int prePage = page - 1;
                if (prePage < 0) prePage = 0;
                int start = pageSize.Value * prePage;
                return start + 1;
            }
        }

        public int EndIndex
        {
            get
            {
                if (!pageSize.HasValue || pageSize <= 0) return records;

                // int size = this.pageSize * this.page;
                int end = pageSize.Value * page;
                if (end <= 0) end = pageSize.Value;
                return end;
            }
        }

        public IGroup filters { get; set; }
    }
}
