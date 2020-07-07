using BlocksCore.Application.Abstratctions;
using BlocksCore.AutoMapper.Abstractions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using Microsoft.Extensions.Localization;
using SysMgt.BussnessDomainModule.Languages;
using SysMgt.BussnessDomainModule.LanguageTexts;
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
    public class LanguagesAppService : AppService,ILanguagesAppService
    {
        public LanguagesDomain LanguagesDomain { get; set; }
        public LanguageTextsDomain LanguageTextsDomain { get; set; }

        public LanguagesAppService(LanguagesDomain languagesDomain, LanguageTextsDomain languageTextsDomain)
        {
            this.LanguagesDomain = languagesDomain;
            this.LanguageTextsDomain = languageTextsDomain;
        }

        public PageList<LanguagesPageResult> GetPageList(LanguagesSearchModel search)
        {
            return LanguagesDomain.GetPageList(search);
        }

        public PageList<LanguageTextsPageResult> GetDetailPageList(LanguageTextsSearchModel search)
        {
            return LanguageTextsDomain.GetPageList(search);
        }
        public string Add(LanguagesInfo languagesInfo) {

            LanguagesData languagesData = new LanguagesData();
            languagesData.Id = Guid.NewGuid().ToString();
            languagesData.LanguageCode = languagesInfo.LanguageCode;
            languagesData.LanguageName = languagesInfo.LanguageName;
            languagesData.LanguageIcon = languagesInfo.LanguageIcon;

            List<LanguageTextsData> details = new List<LanguageTextsData>();
            languagesInfo.LanguageTextsInfos.ForEach(t =>
            {
                LanguageTextsData detail = t.AutoMapTo<LanguageTextsData>();
                details.Add(detail);
            });
            languagesData.LanguageTextsDatas = details;

            return LanguagesDomain.Add(languagesData);
        }

        public string Update(LanguagesInfo languagesInfo) {
            LanguagesData languagesData = new LanguagesData();
            languagesData.Id = languagesInfo.Id;
            languagesData.LanguageCode = languagesInfo.LanguageCode;
            languagesData.LanguageName = languagesInfo.LanguageName;
            languagesData.LanguageIcon = languagesInfo.LanguageIcon;

            List<LanguageTextsData> details = new List<LanguageTextsData>();
            languagesInfo.LanguageTextsInfos.ForEach(t =>
            {
                LanguageTextsData detail = t.AutoMapTo<LanguageTextsData>();
                details.Add(detail);
            });
            languagesData.LanguageTextsDatas = details;

            return LanguagesDomain.Update(languagesData);
        }

        public string Delete(Ids iDS) {
            return LanguagesDomain.Delete(iDS);
        }

        public LanguagesInfo GetOneById(LanguagesInfo languagesInfo) {
            LanguagesData languagesData = new LanguagesData();
            languagesData.Id = languagesInfo.Id;
            languagesData = LanguagesDomain.GetOneById(languagesData);

            languagesInfo.Id = languagesData.Id;
            languagesInfo.LanguageCode = languagesData.LanguageCode;
            languagesInfo.LanguageName = languagesData.LanguageName;
            languagesInfo.LanguageIcon = languagesData.LanguageIcon;
            List<LanguageTextsInfo> languageTextsInfos = new List<LanguageTextsInfo>();
            languagesData.LanguageTextsDatas.ForEach(t => {
                LanguageTextsInfo detail = t.AutoMapTo<LanguageTextsInfo>();
                languageTextsInfos.Add(detail);
            });
            languagesInfo.LanguageTextsInfos = languageTextsInfos;
            return languagesInfo;
        }

        public PageList<ComboboxData> GetComboxList(LanguagesSearchModel search) {
            return LanguagesDomain.GetComboxList(search);
        }
        public PageList<ComboboxData> GetComboxCodeList(LanguagesSearchModel search) {
            return LanguagesDomain.GetComboxCodeList(search);
        }
        //public PageList<ComboboxData> GetModuleComboxList(LanguagesSearchModel search)
        //{
        //    return LanguagesDomain.GetModuleComboxList(search);
        //}

        public PageList<ComboboxData> GetLanguageComboxList(LanguagesSearchModel search)
        {
            return LanguagesDomain.GetLanguageComboxList(search);
        }
    }
}
