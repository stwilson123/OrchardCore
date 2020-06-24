using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Data.Abstractions.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysMgt.BussnessDTOModule.Languages;
using BlocksCore.Abstractions.UI.Combobox;

namespace SysMgt.BussnessRespositoryModule
{
    public interface ILanguagesRepository : IRepository<BDTA_LANGUAGES>
    {
        PageList<LanguagesPageResult> GetPageList(LanguagesSearchModel search);

        PageList<ComboboxData> GetComboxList(LanguagesSearchModel search);

        PageList<ComboboxData> GetComboxCodeList(LanguagesSearchModel search);
    }
}
