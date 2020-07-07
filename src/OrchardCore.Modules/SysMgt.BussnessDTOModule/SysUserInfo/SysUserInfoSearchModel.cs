using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.SysUserInfo
{
    public class SysUserInfoSearchModel
    {
        public Page page { get; set; }
        public string RoleId { get; set; }

        public string State { get; set; }
    }

    public class SysUserRolesSearchModel 
    {
        public string UserCode { get; set; }
    }
}
