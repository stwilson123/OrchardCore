using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.PrintService
{
    public class PrintServiceInfo 
    {
        public string ID { get; set; }
        public string Path { get; set; }
        public long? Flag { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Copies { get; set; }
        public List<Object> PrintList { get; set; }
    }

    public class PrintServiceRetun 
    {
        public string PrintPath { get; set; }
        public string PrintName { get; set; }
        public string PrintType { get; set; }
        public string PRINT_CONTENT_JSONTXT { get; set; }
    }
}
