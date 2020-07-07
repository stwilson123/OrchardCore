using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.ProductFormatDetail
{
   public class ProductFormatDetailInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public long? Length { get; set; }
        public string ProductElementTypeID { get; set; }
        public string ProductElementType { get; set; }

        public string ProductElementTypeIsVariable { get; set; }
        public long? Seq { get; set; }
        public string Default { get; set; }
        public string ProductElementId { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}
