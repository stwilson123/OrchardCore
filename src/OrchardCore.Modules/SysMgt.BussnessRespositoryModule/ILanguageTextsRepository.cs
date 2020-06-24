using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.LanguageTexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessRespositoryModule
{
    public interface ILanguageTextsRepository : IRepository<BDTA_LANGUAGETEXTS>
    {
        PageList<LanguageTextsPageResult> GetPageList(LanguageTextsSearchModel search);

        Task<List<LanguagesDetailData>> GetLanguagesList( string culture);
    }
}
