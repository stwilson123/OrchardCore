using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.ProductElement
{
   public class ProductElementPageResult
    {
        public string ID { get; set; }
        public string ProductElementCode { get; set; }
        public string ProductElementLength { get; set; }
        public string ProductElementName { get; set; }
        public string ProductElementDescription { get; set; }

        public string ProductElementTypeName { get; set; }

        public string AutoIncrement { get; set; }

        public string ResetDate { get; set; }
    }
}
