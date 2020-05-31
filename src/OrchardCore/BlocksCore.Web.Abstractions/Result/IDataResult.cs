using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Result
{
    public interface IDataResult : IResult
    {
        string Code { get; set; }
        object Content { get; set; }
        string Msg { get; set; }
    }
}
