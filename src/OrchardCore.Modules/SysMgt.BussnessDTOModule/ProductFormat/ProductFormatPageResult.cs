using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions.Datatransfer;

namespace SysMgt.BussnessDTOModule.ProductFormat
{
  public class ProductFormatPageResult
    {
        public string ID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? Length { get; set; }
        public string Description { get; set; }
    }
}
