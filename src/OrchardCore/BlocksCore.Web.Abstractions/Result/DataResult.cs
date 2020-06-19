using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Result
{
    public class DataResult : DataResult<object>
    {
    }

    public class DataResult<T> : IDataResult<T>
    {
        public string Code { get; set; }
        public T Content { get; set; }
        public string Msg { get; set; }
    }
}
