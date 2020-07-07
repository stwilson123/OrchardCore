using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Abstractions.Datatransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.Languages
{
    public class LanguagesSearchModel 
    {
        [DataTransfer("page")]
        public Page page { get; set; }
    }
}
