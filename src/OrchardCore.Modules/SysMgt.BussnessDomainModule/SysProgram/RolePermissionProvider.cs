using Blocks.BussnessEntityModule;
using BlocksCore.Infrastructure.Abstractions.Security.Permissions;
using SysMgt.BussnessRespositoryModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.SysProgram
{
    public class RolePermissionProvider : BlocksCore.Infrastructure.Abstractions.Security.Permissions.IPermissionProvider
    {
        private ISysRoleAuthorizeRespository _sysRoleAuthorizeRespository { get; set; }

        public RolePermissionProvider(ISysRoleAuthorizeRespository sysRoleAuthorizeRespository)
        {
            _sysRoleAuthorizeRespository = sysRoleAuthorizeRespository;
        }

        public IDictionary<string, IList<string>> GetPermissions(string roleId = "*")
        {
            var result = roleId == "*" ? _sysRoleAuthorizeRespository.GetRoleAuthorizes() :
                     _sysRoleAuthorizeRespository.GetRoleAuthorize(roleId);
            return transferDic(result);
        }


        IDictionary<string, IList<string>> transferDic(List<SYS_ROLEAUTHORIZE> roleAuthorizes)
        {
            return roleAuthorizes.GroupBy(r => r.SYS_ROLEORUSERID).ToDictionary(r => r.Key, row => row.Select(r => r.RESOURCE_KEY).ToList() as IList<string>);
        }

        public Task<PermissionStereotype> GetStereotype(string name)
        {
            var permissionStereotype = new PermissionStereotype()
            {
                Name = name,
                Permissions = _sysRoleAuthorizeRespository.GetRoleAuthorize(name).Select(t => new Permission(t.RESOURCE_KEY?.ToLower(),t.RESOURCE_KEY?.ToLower()))
            };
            return Task.FromResult(permissionStereotype);
        }
 
    }
}
