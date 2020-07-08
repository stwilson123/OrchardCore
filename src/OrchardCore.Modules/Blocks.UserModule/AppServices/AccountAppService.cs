using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blocks.UserModule.DTO;
using BlocksCore.Abstractions.Security;
using BlocksCore.Domain.Abstractions;
using BlocksCore.Users.Abstractions;
using BlocksCore.Web.Abstractions.Authentication;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Blocks.UserModule.AppServices
{
    public class AccountAppService : IAccountAppService
    {
        private readonly SignInManager<IUser> signInManager;

        //private readonly IAntiforgery antiforgery;
        private readonly IUserContext _userContext;
        private readonly IStringLocalizer T;

        public AccountAppService(SignInManager<IUser> signInManager, IUserContext userContext, IStringLocalizer<AccountAppService> localizer)
        {
            this.signInManager = signInManager;
            this._userContext = userContext;
            this.T = localizer;
        }
        public Task<object> Getdetail()
        {
            var currentUser = _userContext.GetCurrentUser();
            return Task.FromResult<object>(new
            {
                data = new { id = currentUser.UserId, trueName = currentUser.UserAccount, userName = currentUser.UserAccount },
                success = true,
            });

        }
        [Token]
        [AllowAnonymous]
        public async Task<LoginDTO> Login(LoginDTO model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (model.Id == null)
            {
                model.Id = "dac0c8cd-26a0-4c3c-bc6e-5770ef5d8e87";
            }
            if (result.Succeeded)
            {
                return model;
            }
            throw new BlocksBussnessException("101", T[""]);

            //var tokens = antiforgery.GetAndStoreTokens(HttpContext); Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false, Path = "/", IsEssential = true, SameSite = SameSiteMode.Lax }
        }

        public Task LogOff()
        {
            return this.signInManager.SignOutAsync();
        }
    }
}
