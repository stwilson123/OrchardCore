using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blocks.UserModule.DTO;
using BlocksCore.Users.Abstractions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blocks.UserModule.AppServices
{
    public class AccountAppService : IAccountAppService
    {
        private readonly SignInManager<IUser> signInManager;

        //private readonly IAntiforgery antiforgery;
        private readonly IHttpContextAccessor httpContextAccessor;

        //public AccountAppService(IAntiforgery antiforgery,IHttpContextAccessor httpContextAccessor)
        //{
        //    this.antiforgery = antiforgery;
        //    this.httpContextAccessor = httpContextAccessor;
        //}

        public AccountAppService(SignInManager<IUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<LoginDTO> Login(LoginDTO model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return model;
            }
            return model;
            //var tokens = antiforgery.GetAndStoreTokens(HttpContext); Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false, Path = "/", IsEssential = true, SameSite = SameSiteMode.Lax }
        }
    }
}
