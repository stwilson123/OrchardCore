using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Ids;
using SysMgt.BussnessDTOModule.Languages;
using SysMgt.BussnessDTOModule.LanguageTexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessApplicationModule.Languages
{
    public interface ILanguagesAppService : IAppService
    {
        PageList<LanguagesPageResult> GetPageList(LanguagesSearchModel search);

        PageList<LanguageTextsPageResult> GetDetailPageList(LanguageTextsSearchModel search);

        string Add(LanguagesInfo languagesInfo);

        string Delete(Ids iDS);

        string Update(LanguagesInfo languagesInfo);

        LanguagesInfo GetOneById(LanguagesInfo languagesInfo);

        PageList<ComboboxData> GetComboxList(LanguagesSearchModel search);

        PageList<ComboboxData> GetComboxCodeList(LanguagesSearchModel search);
        
        //PageList<ComboboxData> GetModuleComboxList(LanguagesSearchModel search);
        PageList<ComboboxData> GetLanguageComboxList(LanguagesSearchModel search);
    }
}
