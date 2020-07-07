using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Datatransfer;
using SysMgt.BussnessDTOModule.SysProgram;

namespace SysMgt.BussnessDTOModule.SysRoleInfo
{
   public class SysRoleInfoinfo 
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public List<string> IDS { get; set; }
        public List<SysProgramInfo> SysProgramInfos { get; set; }
    }

}
