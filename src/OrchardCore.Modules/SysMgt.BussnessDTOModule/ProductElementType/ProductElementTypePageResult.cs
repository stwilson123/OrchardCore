using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.ProductElementType
{
   public class ProductElementTypePageResult
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string IsVariable { get; set; }
    }
}
