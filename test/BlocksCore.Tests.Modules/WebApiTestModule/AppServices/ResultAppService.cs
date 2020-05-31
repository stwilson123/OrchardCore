using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions.Exception;

namespace WebApiTestModule.AppServices
{
    public class ResultAppService : IResultAppService
    {
        public object GetObject(object obj)
        {
            return obj;
        }

        public object GetObjectWhenBlocksException(object obj)
        {
            throw new BlocksException("101","BlocksException",obj);
        }

        public object GetObjectWhenException(object obj)
        {
            throw new NotImplementedException();
        }

        public string GetValue(string value)
        {
            return value;
        }
    }
   
}
