using Blocks.BussnessEntityModule;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Repository;
using System;
using System.Collections.Generic;
using BlocksCore.Data.EF.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.LanguageTexts;
using SysMgt.BussnessDTOModule.Languages;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule
{
    public class LanguageTextsRepository : DBSqlRepositoryBase<BDTA_LANGUAGETEXTS>, ILanguageTextsRepository
    {
        public LanguageTextsRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        public PageList<LanguageTextsPageResult> GetPageList(LanguageTextsSearchModel search)
        {
            return GetContextTable().Where((BDTA_LANGUAGETEXTS languagetexts) => languagetexts.LANGUAGE_ID == search.LanguageID).Paging((BDTA_LANGUAGETEXTS languagetexts) => new LanguageTextsPageResult()
            {
                Id = languagetexts.Id,
                LanguageModule = languagetexts.LANGUAGE_MODULE,
                LanguageId = languagetexts.LANGUAGE_ID,
                LanguageCode = languagetexts.LANGUAGE_CODE,
                LanguageKey = languagetexts.LANGUAGE_KEY,
                LanguageValue = languagetexts.LANGUAGE_VALUE,
                LanguageName = languagetexts.BDTA_LANGUAGES.LANGUAGE_NAME,
                LanguageIcon = languagetexts.BDTA_LANGUAGES.LANGUAGE_ICON
            }, search.page);
        }

    
        public Task<List<LanguagesDetailData>> GetLanguagesList( string culture)        {
            return GetContextTable()
                         .Where((BDTA_LANGUAGETEXTS t) => t.LANGUAGE_CODE == culture)

                         .SelectToListAsync((BDTA_LANGUAGETEXTS t) => new LanguagesDetailData
                         {
                             LanguageKey = t.LANGUAGE_KEY,
                             LanguageValue = t.LANGUAGE_VALUE,
                             ModuleName = t.LANGUAGE_MODULE
                         });
        }
    }
}
