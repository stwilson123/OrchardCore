//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security;
//using System.Text;
//using System.Threading.Tasks;
//using Blocks.Core.Security.Authorization.Permission;
//using BlocksCore.Abstractions.Security;
//using Blocks.Framework.Security.Authorization.Permission;
//using SysMgt.BussnessRespositoryModule;
//using BlocksCore.Infrastructure.Abstractions.Security.Permissions;

//namespace SysMgt.BussnessDomainModule.SysProgram
//{
//	public class PermissionChecker : IPermissionCheck
//	{

//		public ISysProgramOperationRepository SysProgramOperationRepository { get; set; }
//		public ISysRoleUserRepository SysRoleUserRepository { get; set; }

//		public ISysRoleAuthorizeRespository SysRoleAuthorizeRespository { get; set; }

//		private IPermissionManager _permissionManager { get; set; }

//		public PermissionChecker(IPermissionManager permissionManager, ISysRoleUserRepository sysRoleUserRepository, ISysRoleAuthorizeRespository SysRoleAuthorizeRespository)
//		{
//			this._permissionManager = permissionManager;
//			this.SysRoleUserRepository = sysRoleUserRepository;
//			this.SysRoleAuthorizeRespository = SysRoleAuthorizeRespository;
//		}

//		public Task<bool> IsGrantedAsync(Blocks.Framework.Security.IUserIdentifier user, Blocks.Framework.Security.Authorization.Permission.IPermission permission)
//		{
//            if (user == null)
//                return Task.FromResult(false);
//            var sysRoleUser = SysRoleUserRepository.FirstOrDefault(t => t.SYS_USERINFOID == user.UserId);
//            if (sysRoleUser == null)
//            {
//                return Task.FromResult(false);
//            }

//            var roleId = user.RoleIds.FirstOrDefault();
//            if (roleId != null)
//            {
//                var rolePermissions = _permissionManager.GetAllPermissions();
//                if (!rolePermissions.ContainsKey(roleId))
//                    return Task.FromResult(false);
//                var result = rolePermissions[roleId].Any(p => p.ResourceKey == permission.ResourceKey);


//                return Task.FromResult(result);
//            }
//            return Task.FromResult(false);
//		}
//	}
//}