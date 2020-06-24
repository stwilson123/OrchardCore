using BlocksCore.Domain.Abstractions.Domain;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.LanguageTexts;
using SysMgt.BussnessRespositoryModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.LanguageTexts
{
    public class LanguageTextsDomain : IDomainService
    {
        public ILanguageTextsRepository LanguageTextsRepository { get; set; }

        public LanguageTextsDomain(ILanguageTextsRepository languageTextsRepository)
        {
            this.LanguageTextsRepository = languageTextsRepository;
        }

        public virtual PageList<LanguageTextsPageResult> GetPageList(LanguageTextsSearchModel search)
        {
            return LanguageTextsRepository.GetPageList(search);
        }
    }
}
