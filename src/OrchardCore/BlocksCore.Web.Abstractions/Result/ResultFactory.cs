using System;
using BlocksCore.Abstractions.Exception;

namespace BlocksCore.Web.Abstractions.Result
{
    public class ResultFactory
    {
        public static IResult CreateDataResult(object result, Exception exception)
        {
            if (result is IResult)
                return result as IResult;

            if (exception != null)
            {
                if(exception is BlocksException blocksException)
                {
                    return new DataResult() { Code = blocksException.Code, Content = blocksException.Content, Msg = blocksException.Message };
                }

                return new DataResult() { Code = "500", Msg = exception.Message };
            }
                

            return new DataResult() { Code = ((int)ResultCode.Success).ToString(), Content = result };
        }
    }
}
