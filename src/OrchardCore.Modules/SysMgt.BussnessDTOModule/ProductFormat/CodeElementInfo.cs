using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions.Datatransfer;

namespace SysMgt.BussnessDTOModule.ProductFormat
{
   public class CodeElementInfo 
    {
        public Dictionary<string, string> Dic { get; set; }

        public string Code { get; set; }

        public int Number { get; set; } = 1;
    }
}
