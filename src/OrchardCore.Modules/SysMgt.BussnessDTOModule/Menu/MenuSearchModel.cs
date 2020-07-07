using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.Menu
{
    public class MenuSearchModel
    {
        [DataTransfer("page")]
        public Page page { get; set; }
    }
}
