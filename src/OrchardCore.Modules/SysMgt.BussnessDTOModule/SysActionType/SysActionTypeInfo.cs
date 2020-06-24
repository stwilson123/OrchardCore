using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions.Datatransfer;

namespace SysMgt.BussnessDTOModule.SysActionType
{
  public  class SysActionTypeInfo
    {
        public string ID { get; set; }
        public string TypeCode { get; set; }

        public string TypeName { get; set; }
    }
}
