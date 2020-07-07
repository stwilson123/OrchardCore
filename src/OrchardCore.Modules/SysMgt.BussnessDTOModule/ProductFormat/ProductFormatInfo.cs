using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Datatransfer;
using SysMgt.BussnessDTOModule.ProductElement;

namespace SysMgt.BussnessDTOModule.ProductFormat
{
   public class ProductFormatInfo 
    {
        public List<string> IDS { get; set; }
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductElementInfo> ProductElements { get; set; }
    }

    public class ProductElementInfo
    {
        public string ID { get; set; }
        public string ProductElementID { get; set; }
        public string ProductElementLength { get; set; }
        public string ProductElementName { get; set; }
        public string ProductformatStart { get; set; }
        public string ProductformatEnd { get; set; }
    }
}
