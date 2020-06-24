using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions.Datatransfer;

namespace SysMgt.BussnessDTOModule.Menu
{
   public class MenuPageResult
    {
        public string ID { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }

        public string Desc { get; set; }

        public long Sort { get; set; }
    }
}
