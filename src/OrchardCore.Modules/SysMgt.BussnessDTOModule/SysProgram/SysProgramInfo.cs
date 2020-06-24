using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Localization.Abtractions;
using BlocksCore.Application.Abstratctions.Datatransfer;

namespace SysMgt.BussnessDTOModule.SysProgram
{
   public class SysProgramInfo
    {
        public string PID { get; set; }
        public string ID { get; set; }
        [LocalizedDescription("CODE")]
        public string Code { get; set; }
        [LocalizedDescription("NAME")]
        public string Name { get; set; }
        [LocalizedDescription("URL")]
        public string URL { get; set; }

        public string Sort { get; set; }

        public string Type { get; set; }
    }
}
