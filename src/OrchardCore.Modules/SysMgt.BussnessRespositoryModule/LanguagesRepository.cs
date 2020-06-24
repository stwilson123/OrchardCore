using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Repository;
using SysMgt.BussnessDTOModule.Languages;
using System;
using System.Collections.Generic;
using BlocksCore.Data.EF.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule
{
    public class LanguagesRepository : DBSqlRepositoryBase<BDTA_LANGUAGES>, ILanguagesRepository
    {
        public LanguagesRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        public PageList<LanguagesPageResult> GetPageList(LanguagesSearchModel search)
        {
            return GetContextTable().Paging((BDTA_LANGUAGES languages) => new LanguagesPageResult()
            {
                Id = languages.Id,
                LanguageCode = languages.LANGUAGE_CODE,
                LanguageName = languages.LANGUAGE_NAME,
                LanguageIcon = languages.LANGUAGE_ICON
            }, search.page);
        }

        public PageList<ComboboxData> GetComboxList(LanguagesSearchModel search)
        {
            return GetContextTable().Paging((BDTA_LANGUAGES languages) => new ComboboxData()
            {
                Id = languages.Id,
                Text = languages.LANGUAGE_NAME
            }, search.page);
        }

        public PageList<ComboboxData> GetComboxCodeList(LanguagesSearchModel search)
        {
            return GetContextTable().Paging((BDTA_LANGUAGES languages) => new ComboboxData()
            {
                Id = languages.Id,
                Text = languages.LANGUAGE_CODE
            }, search.page);
        }
    }
}
