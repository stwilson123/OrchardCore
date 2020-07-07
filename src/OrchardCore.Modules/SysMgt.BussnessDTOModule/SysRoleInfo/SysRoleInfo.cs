using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Localization.Abtractions;
using BlocksCore.Abstractions.Datatransfer;
using SysMgt.BussnessDTOModule.SysProgram;

namespace SysMgt.BussnessDTOModule.SysRoleInfo
{
   public class SysRoleInfo
    {
        public string ID { get; set; }
        [LocalizedDescription("NAME")]
        public string Name { get; set; }
        [LocalizedDescription("REMARK")]
        public string Remark { get; set; }
        [LocalizedDescription("Ids")]
        public List<string> IDS { get; set; }
        [LocalizedDescription("menu_management")]
        public List<SysProgramInfo> SysProgramInfos { get; set; }
    }

    public class SysRoleUserInfo 
    {
        [LocalizedDescription("ID")]
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserInfoID{ get; set; }
        [LocalizedDescription("Ids")]
        /// <summary>
        /// 用户对应角色已授权ID集合
        /// </summary>
        public List<string> Ids { get; set; }
    }
}
