using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BlocksCore.Infrastructure.Abstractions.Security.Permissions;
using Microsoft.AspNetCore.Authorization;
using OrchardCore.Security;
using Microsoft.Extensions.DependencyInjection;
using BlocksCore.Abstractions.Security;

namespace BlocksCore.Users.Services
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserContext _userContext;

        public PermissionHandler(IServiceProvider serviceProvider,IUserContext userContext)
        {
            this._serviceProvider = serviceProvider;
            this._userContext = userContext;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (!(bool)context?.User?.Identity?.IsAuthenticated)
            {
                return;
            }

            if(this._serviceProvider.GetAutofacRoot().IsRegistered<IPermissionChecker>())
            {
                var permissionChecker = _serviceProvider.GetService<IPermissionChecker>();
                if(await permissionChecker.IsGrantedAsync(_userContext.GetCurrentUser(), requirement.Permission.ToPermision()))
                {
                    context.Succeed(requirement);
                }
            }

           
            return;
        }
    }
}
