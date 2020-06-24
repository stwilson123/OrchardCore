using OrchardCore.Localization;
using SysMgt.BussnessDTOModule.LanguageTexts;
using SysMgt.BussnessRespositoryModule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SysMgt.BussnessDomainModule.Languages
{
    //public class LanguagesLocalizationProvider : ILocalizationProvider
    //{
    //    public ILanguageTextsRepository LanguageTextsRepository { get; set; }
    //    public LanguagesLocalizationProvider(ILanguageTextsRepository languageTextsRepository)
    //    {
    //        this.LanguageTextsRepository = languageTextsRepository;
    //    }
    //    public async Task<IDictionary<string, string>> getLocalizationDicionary(string moduleName, string culture)
    //    {



    //        List<LanguagesDetailData> details = new List<LanguagesDetailData>();
    //        details = await LanguageTextsRepository.GetLanguagesList(culture);            
    //        return details.ToDictionary(LanguagesDetailData => LanguagesDetailData.LanguageKey, LanguagesDetailData => LanguagesDetailData.LanguageValue);
    //        //throw new NotImplementedException();
    //    }
    //}

    public class LanguagesLocalizationProvider : ITranslationProvider
    {
        public ILanguageTextsRepository LanguageTextsRepository { get; set; }
        public LanguagesLocalizationProvider(ILanguageTextsRepository languageTextsRepository)
        {
            this.LanguageTextsRepository = languageTextsRepository;
        }
        public async Task<IDictionary<string, string>> getLocalizationDicionary(string moduleName, string culture)
        {



            List<LanguagesDetailData> details = new List<LanguagesDetailData>();
            details = await LanguageTextsRepository.GetLanguagesList(culture);
            return details.ToDictionary(LanguagesDetailData => LanguagesDetailData.LanguageKey, LanguagesDetailData => LanguagesDetailData.LanguageValue);
            //throw new NotImplementedException();
        }

        public async void LoadTranslations(string cultureName, CultureDictionary dictionary)
        {
            List<LanguagesDetailData> details = await LanguageTextsRepository.GetLanguagesList(cultureName);

            dictionary.MergeTranslations(details.Select(d => new CultureDictionaryRecord(d.LanguageKey,"", new[] { d.LanguageValue })));
        }
    }
}