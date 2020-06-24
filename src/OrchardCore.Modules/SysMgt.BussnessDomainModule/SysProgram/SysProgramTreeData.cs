using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.SysProgram
{
   public class SysProgramTreeData
    {
        public string type { get; set; }//0:菜单，1：按钮
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public bool @checked { get; set; }

        public string url { get; set; }

        public string urlkey { get; set; }
    }

}
