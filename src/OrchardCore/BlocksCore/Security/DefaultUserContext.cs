using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions.Security;
using BlocksCore.Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace BlocksCore.Security
{
    public class DefaultUserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;

        public DefaultUserContext(IHttpContextAccessor httpContextAccessor,IServiceProvider serviceProvider)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._serviceProvider = serviceProvider;
        }
        public IUserIdentifier GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            IEnumerable<string> roleIds = new List<string>();
            if(_serviceProvider.IsRegistered<IDentityUserStore>())
            {
                roleIds = _serviceProvider.GetService<IDentityUserStore>().GetRoles(userId);
            }
            return new DefaultUserIdentifier("", userId, user.Identity.Name, roleIds);
        }
    }
}
