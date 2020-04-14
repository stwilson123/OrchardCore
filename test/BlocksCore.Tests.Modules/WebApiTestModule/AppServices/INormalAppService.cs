using System;
using System.Collections.Generic;
using System.Text;
using WebApiTestModule.DTO;
using BlocksCore.Application.Abstratctions;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTestModule
{
    public interface INormalAppService : IAppService
    {
        string GetUserName(UserDTO a);

        object DefaultMethod(object inputObj);

        [HttpGet]
        object GetMethod(Int32 int32, Int64 int64, String str, DateTime date, float fl);

        object PostMethod(object inputObj);

    }



}
