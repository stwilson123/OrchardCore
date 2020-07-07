//using Microsoft.Extensions.Localization;
//using Blocks.Framework.Security.Authorization;
//using SysMgt.BussnessDomainModule.Common;
//using SysMgt.BussnessRespositoryModule;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SysMgt.BussnessDomainModule.SysUserInfo
//{
//   public class DefaultUserPasswordData: DefaultUserPassword
//    {
//        private ISysUserInfoRepository SysUserInfoRepository { get; set; }
 
//        public DefaultUserPasswordData(ISysUserInfoRepository sysUserInfoRepository)
//        {
//            this.SysUserInfoRepository = sysUserInfoRepository;
//        }

//        public override bool validate(string userAccount, string password)
//        {
//            var userInfo = SysUserInfoRepository.FirstOrDefault(t => t.USERCODE == userAccount);
//            if (userInfo == null)
//            {
//                return false;    //HelperBLL.ThrowEx("101", L["账号不存在！"]);            
//            }
//            if (userInfo.PASSWORD != password)
//            {
//                return false; // HelperBLL.ThrowEx("101", L["账号密码不正确！"]);              
//            }
//            return true;
//        }
//    }
//}
