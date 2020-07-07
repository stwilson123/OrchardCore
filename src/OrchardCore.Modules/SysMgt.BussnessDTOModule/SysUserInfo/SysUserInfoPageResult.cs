using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.SysUserInfo
{
    public class SysUserInfoPageResult
    {
        public string ID { get; set; }
        public string UserCode { get; set; }
        public string CName { get; set; }
        public string Password { get; set; }
        public string State { get; set; }
        public string Memo { get; set; }
        public string Roles { get; set; }
    }
}
