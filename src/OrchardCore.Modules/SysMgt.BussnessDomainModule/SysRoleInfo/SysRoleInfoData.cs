using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysMgt.BussnessDomainModule.SysProgram;

namespace SysMgt.BussnessDomainModule.SysRoleInfo
{
   public class SysRoleInfoData
    {
        public string ID { get; set; }

        public string Name { get; set; }
        public List<string> IDS { get; set; }
        public string Remark { get; set; }

        public List<SysProgramData> SysProgramDatas { get; set; }
    }
}
