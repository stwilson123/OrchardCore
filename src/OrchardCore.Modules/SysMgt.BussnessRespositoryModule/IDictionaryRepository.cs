using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Combobox;

namespace SysMgt.BussnessRespositoryModule
{
    public interface IDictionaryRepository : IRepository<BDTA_DICTIONARY>
    {
        PageList<ComboboxData> GetComboxList(SearchModel search);
    }
}
