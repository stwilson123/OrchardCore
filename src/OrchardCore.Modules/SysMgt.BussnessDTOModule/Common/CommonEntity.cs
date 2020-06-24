using BlocksCore.Application.Abstratctions.Datatransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.Common
{
    public class CommonEntity 
    {
        /// <summary>
        /// ID集合
        /// </summary>
        public List<string> IDs { get; set; }
         
        public string ID { get; set; }

        /// <summary>
        /// 任意字符串
        /// </summary>
        public string STR { get; set; }
    }
}
