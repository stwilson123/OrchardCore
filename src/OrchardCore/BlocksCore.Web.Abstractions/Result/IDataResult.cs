using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Result
{
    public interface IDataResult<T> : IDataResult
    {
        string Code { get; set; }
        T Content { get; set; }
        string Msg { get; set; }
    }


    public interface IDataResult : IResult
    {

    }
}
