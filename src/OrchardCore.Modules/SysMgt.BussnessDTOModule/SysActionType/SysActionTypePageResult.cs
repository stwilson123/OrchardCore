using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.SysActionType
{
   public class SysActionTypePageResult
    {
        public string ID { get; set; }
        public string TypeCode { get; set; }

        public string TypeName { get; set; }
    }
}
