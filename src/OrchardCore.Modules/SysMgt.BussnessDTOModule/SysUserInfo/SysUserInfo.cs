using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Localization.Abtractions;
using BlocksCore.Application.Abstratctions.Datatransfer;

namespace SysMgt.BussnessDTOModule.SysUserInfo
{
   public class SysUserInfo
    {
        [LocalizedDescription("ID")]
        public string ID { get; set; }
        [LocalizedDescription("CODE")]
        public string UserCode { get; set; }
        [LocalizedDescription("NAME")]
        public string CName { get; set; }
        [LocalizedDescription("PASSWORD")]
        public string Password { get; set; }
        [LocalizedDescription("STATE")]
        public long? State { get; set; }
        [LocalizedDescription("REMARK")]
        public string Memo { get; set; }
       
    }
    public class SysRoleAndUserInfo 
    {
        [LocalizedDescription("ID")]
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleInfoID { get; set; }
        [LocalizedDescription("Ids")]
        /// <summary>
        /// 角色绑定用户ID集合
        /// </summary>
        public List<string> Ids { get; set; }
    }

    public class SysUserRoles
    {

        public string USERCODE { get; set; }

        public string USERID { get; set; }

        public List<string> UserRoles { get; set; }

    }

    public class SysUserPwdModificationInfo 
    {
       
        public string ID { get; set; }
     
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }


        public string ConfirmPassword { get; set; }
    }
}
