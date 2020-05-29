using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Filters
{
    public interface IOrderedFilter
    {
        int Order { get; }
    }
}
