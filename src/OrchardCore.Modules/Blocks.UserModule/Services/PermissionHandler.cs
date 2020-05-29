using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;

namespace BlocksCore.Users.Services
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (!(bool)context?.User?.Identity?.IsAuthenticated)
            {
                return Task.CompletedTask;
            }
           
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
