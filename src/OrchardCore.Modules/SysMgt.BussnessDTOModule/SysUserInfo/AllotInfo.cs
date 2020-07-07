using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BlocksCore.Localization.Abtractions;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.SysUserInfo
{
   public class AllotInfo
    {
        [LocalizedDescription("ID")]
        public string RoleID { get; set; }
        [LocalizedDescription("Ids")]
        public List<string> IDs { get; set; }
    }
}