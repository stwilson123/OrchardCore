using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Abstractions.Data.Paging
{
    public interface IPage
    {
        int? pageSize { get; set; }

        int page { get; set; }

        //TODO will be multi sort.
        string sortColumn { get; set; }

        //TODO will be multi sort.
        string sortOrder { get; set; }

        int total { get; }

        int records { get; set; }

        int StartIndex { get; }

        int EndIndex { get; }

        IGroup filters { get; set; }
    }
}
