using System;
using System.Collections.Generic;
using System.Text;
using Blocks.UserModule.DTO;
using BlocksCore.Application.Abstratctions;
using Microsoft.AspNetCore.Mvc;

namespace Blocks.UserModule.AppServices
{
    public interface IUserAppService : IAppService
    {
        string GetUserName(UserDTO a);


        [IgnoreAntiforgeryToken]
        string PostData();
    }



}
