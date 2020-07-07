using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.ProductFormatDetail
{
   public class ProductFormatDetailPageResult
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public long? Length { get; set; }
        public long? Seq{ get; set; }
        public string ProductformatStart { get; set; }
        public string ProductformatEnd { get; set; }
    }
}
