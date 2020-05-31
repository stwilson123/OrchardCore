using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Result
{
    public class DataResult : IDataResult
    {
        public string Code { get ; set ; }
        public object Content { get ; set ; }
        public string Msg { get ; set ; }
    }
}
