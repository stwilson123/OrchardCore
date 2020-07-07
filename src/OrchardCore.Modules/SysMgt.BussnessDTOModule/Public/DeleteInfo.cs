using BlocksCore.Abstractions.Datatransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.Public
{
    public class DeleteInfo 
    {

        public List<string> IDS { get; set; }
        public string ID { get; set; }
        public string XmlPatch { get; set; }


    }
}
