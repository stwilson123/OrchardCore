using System;
using System.Collections.Generic;
using System.Text;
using WebApiTestModule.DTO;
using BlocksCore.Web.Abstractions.HttpMethod;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTestModule
{
    //[ApiController]
    // [Route("{area:exists}/{controller}/{action}")]
    //[Area("WebApiTestModule/api")]
    public class NormalAppService : INormalAppService
    {
        //[BlocksAction]
        public string GetUserName(UserDTO a)
        {
            throw new NotImplementedException();
        }


        [IgnoreAntiforgeryToken]
        public object DefaultMethod(object inputObj)
        {
            return inputObj;
        }


        [IgnoreAntiforgeryToken]
        public object GetMethod(Int32 int32, Int64 int64, String str, DateTime date, float fl)
        {
            return new Dictionary<string, object>() { { "date" , date },
                { "int32" , int32 },
                { "int64" , int64 },
                { "fl" , 1.0 }, };
        }


        [IgnoreAntiforgeryToken]
       // [HttpGet,HttpPost]
        public object PostMethod(object inputObj)
        {
            return inputObj;
        }
    }


}
