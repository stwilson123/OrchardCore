using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Abstractions.Datatransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.Combobox
{
    public class SearchModel 
    {
        public string DictionaryTypeCode { get; set; }
        [DataTransfer("page")]
        public Page page { get; set; }
    }
}
