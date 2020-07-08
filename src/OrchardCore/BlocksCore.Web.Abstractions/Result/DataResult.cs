using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions.Datatransfer;

namespace BlocksCore.Web.Abstractions.Result
{
    public class DataResult : DataResult<object>
    {
    }

    public class DataResult<T> : IDataResult<T>
    {
        [DataTransfer("code")]
        public string Code { get; set; }

        [DataTransfer("content")]
        public T Content { get; set; }

        [DataTransfer("msg")]
        public string Msg { get; set; }
    }
}
