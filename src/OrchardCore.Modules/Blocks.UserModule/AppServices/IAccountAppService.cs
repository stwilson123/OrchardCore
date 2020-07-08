using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blocks.UserModule.DTO;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Web.Abstractions.Authentication;

namespace Blocks.UserModule.AppServices
{
    public interface IAccountAppService: IAppService,IAuthenticationService
    {
        Task<LoginDTO> Login(LoginDTO model);


        Task LogOff();

        Task<object> Getdetail();
    }
}
