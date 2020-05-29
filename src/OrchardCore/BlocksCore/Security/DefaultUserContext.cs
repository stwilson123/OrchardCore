using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions.Security;
using Microsoft.AspNetCore.Http;

namespace BlocksCore.Security
{
    public class DefaultUserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultUserContext(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public IUserIdentifier GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext.User;
            return new DefaultUserIdentifier("", user.FindFirst("Id")?.Value, user.Identity.Name, null);
        }
    }
}
