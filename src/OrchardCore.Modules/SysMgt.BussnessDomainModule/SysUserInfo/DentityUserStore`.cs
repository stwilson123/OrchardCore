
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using BlocksCore.Abstractions.Security;
using SysMgt.BussnessDomainModule.Common;
using SysMgt.BussnessRespositoryModule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.SysUserInfo
{
    public class DentityUserStore : IDentityUserStore
    {
        private ISysUserInfoRepository SysUserInfoRepository { get; set; }
        private ISysRoleUserRepository SysRoleUserRepository { get; set; }
        public IStringLocalizer L { get; set; }

       // public ILog log { get; set; }

        public DentityUserStore(ISysUserInfoRepository sysUserInfoRepository, ISysRoleUserRepository sysRoleUserRepository)
        {
            this.SysUserInfoRepository = sysUserInfoRepository;
            this.SysRoleUserRepository = sysRoleUserRepository;
        }


        public IEnumerable<string> GetRoles(string userId)
        {
            return this.SysRoleUserRepository.GetAll().Where(r => r.SYS_USERINFOID == userId).Select(r => r.SYS_ROLEINFOID);
        }

        //public void CheckUserStatus(IUserIdentifier userIdentifier)
        //{
        //    var userId = userIdentifier.UserId;
        //    var userInfo = SysUserInfoRepository.FirstOrDefault(n => n.Id == userId);
        //    if (userInfo != null)
        //    {
        //        if (userInfo.STATE == 2)
        //        {
        //            HelperBLL.ThrowEx("101", L(L["InvalidUserState2"].AutoMapTo<string>()));
        //        }
        //    }
        //    else
        //    {
        //        HelperBLL.ThrowEx("101", L(L["InvalidUserState2"].AutoMapTo<string>()));
        //    }
        //}
    }
}