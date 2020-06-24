using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.PrintService
{
   public class PrintServiceData
    {
        public string ID { get; set; }
        public string Path { get; set; }
        public long? Flag { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public string JsonTxt { get; set; }
    }
}
