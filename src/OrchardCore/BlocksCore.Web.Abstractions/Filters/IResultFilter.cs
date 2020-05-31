using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Filters
{
    public interface IResultFilter
    {
     
        void OnResultExecuted(ResultExecutedContext context);
 
        void OnResultExecuting(ResultExecutingContext context);

    }
}
