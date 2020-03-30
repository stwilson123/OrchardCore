using System;
using System.Collections.Generic;
using System.Text;
using Blocks.UserModule.DTO;
using BlocksCore.Web.Abstractions.HttpMethod;
using Microsoft.AspNetCore.Mvc;

namespace Blocks.UserModule.AppServices
{
    public class UserAppService : IUserAppService
    {
        //[BlocksAction]
        public string GetUserName(UserDTO a)
        {
            throw new NotImplementedException();
        }
        [IgnoreAntiforgeryToken]

        public string PostData()
        {
            return "1";
        }
    }


}
