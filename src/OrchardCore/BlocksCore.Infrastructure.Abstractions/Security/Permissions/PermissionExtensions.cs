using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Infrastructure.Abstractions.Security.Permissions
{
    public static class PermissionExtensions
    {
        public static Permission ToPermision(this OrchardCore.Security.Permissions.Permission permission)
        {
            return new Permission(permission.Name, permission.Name) {
                Description = permission.Description
            };
        }
    }
}
