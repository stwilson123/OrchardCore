using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Application.Abstratctions;

namespace WebApiTestModule.AppServices
{
    public interface IResultAppService : IAppService
    {
        string GetValue(string value);

        object GetObject(object obj);

        object GetObjectWhenException(object obj);

        object GetObjectWhenBlocksException(object obj);

    }
}
