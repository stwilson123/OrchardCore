using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.ProductElementType
{
    public class ProductElementTypeData
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string IsVariable { get; set; }
        public List<string> IDS { get; set; }
    }
}
