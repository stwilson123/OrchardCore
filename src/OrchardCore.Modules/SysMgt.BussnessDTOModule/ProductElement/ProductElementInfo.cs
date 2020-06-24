using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions.Datatransfer;

namespace SysMgt.BussnessDTOModule.ProductElement
{
    public class ProductElementInfo
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<string> IDS { get; set; }
        public string Length { get; set; }
        public string Default { get; set; }
        public string Description { get; set; }
        public string ElementTypeId { get; set; }
        public string ResetDate { get; set; }
        public string AutoIncrement { get; set; }
    }
}
