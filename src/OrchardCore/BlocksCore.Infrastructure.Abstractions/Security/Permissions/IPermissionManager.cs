using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace BlocksCore.Infrastructure.Abstractions.Security.Permissions
{
    public interface IPermissionManager
    {
        //Task<IEnumerable<Permission>> GetPermissionsAsync();

      //  Task<Permission> GetPermissionsAsync(string permissionName);

        Task<IDictionary<string, Permission>> GetPermissionsAsync(string permissionStereotypeName);
    }
}
