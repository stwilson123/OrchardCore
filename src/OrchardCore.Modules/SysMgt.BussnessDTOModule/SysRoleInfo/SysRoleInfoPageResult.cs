using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions.Datatransfer;

namespace SysMgt.BussnessDTOModule.SysRoleInfo
{
   public class SysRoleInfoPageResult
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
    }
}
