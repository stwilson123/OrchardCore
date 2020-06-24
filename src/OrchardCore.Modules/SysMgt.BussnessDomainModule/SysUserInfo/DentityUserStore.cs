
//using BlocksCore.Localization.Abtractions;
//using Microsoft.Extensions.Logging;
//using BlocksCore.Abstractions.Security;
//using Blocks.Framework.Security.Authorization;
//using SysMgt.BussnessDomainModule.Common;
//using SysMgt.BussnessRespositoryModule;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SysMgt.BussnessDomainModule.SysUserInfo
//{
//	public class DentityUserStore : IDentityUserStore
//	{
//		private ISysUserInfoRepository SysUserInfoRepository { get; set; }
//		private ISysRoleUserRepository SysRoleUserRepository { get; set; }
//		public Localizer L { get; set; }

//		public ILog log { get; set; }

//		public DentityUserStore(ISysUserInfoRepository sysUserInfoRepository, ISysRoleUserRepository sysRoleUserRepository)
//		{
//			this.SysUserInfoRepository = sysUserInfoRepository;
//			this.SysRoleUserRepository = sysRoleUserRepository;
//		}

//		public UserIdentifier GetUser(string UserAccount)
//		{
//			Stopwatch sw = Stopwatch.StartNew();
//			//var userInfo = SysUserInfoRepository.FirstOrDefault(t => t.USERCODE == UserAccount);
//			//if (userInfo == null)
//			//{
//			//	HelperBLL.ThrowEx("101", L(L("InvalidUserNameOrPassword").AutoMapTo<string>()));
//			//}
//			//var userRoles = SysRoleUserRepository.GetAllList(u => u.SYS_USERINFOID == userInfo.Id)
//			//	.Select(r => r.SYS_ROLEINFOID).ToList();

//            var userInfo = SysUserInfoRepository.GetUserRoles(new BussnessDTOModule.SysUserInfo.SysUserRolesSearchModel()
//            {
//                UserCode = UserAccount
//            }).FirstOrDefault();

//            if (userInfo == null)
//            {
//                HelperBLL.ThrowEx("101", L(L("InvalidUserNameOrPassword").AutoMapTo<string>()));
//            }

//            UserIdentifier userIdentifier = new UserIdentifier(userInfo.USERID, null, userInfo.USERCODE, userInfo.UserRoles);
//			sw.Stop();
//			log.Logger(new LogModel() { Message = string.Format("GetUser Cost {0}ms", sw.ElapsedMilliseconds) });
//			return userIdentifier;
//		}

//		public void CheckUserStatus(IUserIdentifier userIdentifier)
//		{
//			var userId = userIdentifier.UserId;
//			var userInfo = SysUserInfoRepository.FirstOrDefault(n => n.Id == userId);
//			if (userInfo != null)
//			{
//				if (userInfo.STATE == 2)
//				{
//					HelperBLL.ThrowEx("101", L(L("InvalidUserState2").AutoMapTo<string>()));
//				}
//			}
//			else
//			{
//				HelperBLL.ThrowEx("101", L(L("InvalidUserState2").AutoMapTo<string>()));
//			}
//		}
//	}
//}