using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.SysProgram
{
   public  class SysPogramTree
    {
        public string type { get; set; }
        public string id { get; set; }
        public string pId { get; set; }
        public bool @checked { get; set; }
        public string name { get; set; }

        public string url { get; set; }

        public string urlkey { get; set; }
    }

    public class ELSysPogramTree 
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool @checked {get;set;}

        public string URL { get; set; }
        public string type { get; set; }
        public List<ELSysPogramTree> children { get; set; }
        public string PID { get; set; }
        public string urlkey { get; set; }
    }

    public class ELsysPogramTreeCheckedNode 
    {
        public List<ELSysPogramTree> ELSysPogramTreeDatas { get; set; }
        public List<string> TreeCheckedNodeIDs { get; set; }

    }
}
