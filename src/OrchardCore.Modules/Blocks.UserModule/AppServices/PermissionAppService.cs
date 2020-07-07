using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Security;
using BlocksCore.Infrastructure.Abstractions.Security.Permissions;

namespace BlocksCore.Users.AppServices
{
    public class PermissionAppService : IPermissionAppService
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IUserContext _userContext;

        public PermissionAppService(IPermissionManager permissionManager,IUserContext userContext)
        {
            this._permissionManager = permissionManager;
            this._userContext = userContext;
        }
        public async Task<IEnumerable<string>> Get()
        {
            var roleIds = _userContext.GetCurrentUser()?.RoleIds;
            var permissions = new List<string>();
            foreach (var roleId in roleIds)
            {
                var rolePermissions = (await this._permissionManager.GetPermissionsAsync(roleId)).Values.Select(p => p.ResourceKey);
                permissions.AddRange(rolePermissions);
            }
            return permissions;
        }
    }
}
