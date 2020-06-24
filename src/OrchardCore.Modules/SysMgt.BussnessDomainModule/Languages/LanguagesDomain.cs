using BlocksCore.Domain.Abstractions.Domain;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions;
using BlocksCore.Localization.Abtractions;
using SysMgt.BussnessDomainModule.LanguageTexts;
using SysMgt.BussnessDTOModule.Ids;
using SysMgt.BussnessDTOModule.Languages;
using SysMgt.BussnessRespositoryModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.Languages
{
    public class LanguagesDomain : IDomainService
    {
        public ILanguagesRepository LanguagesRepository { get; set; }
        public ILanguageTextsRepository LanguageTextsRepository { get; set; }
        //public IExtensionsWrapper ExtensionsWrapper { get; set; }
        public ILanguageManager LanguagesManager { get; set; }

        public Localizer L { get; set; }

        public LanguagesDomain(ILanguagesRepository languagesRepository, ILanguageTextsRepository languageTextsRepository, /*IExtensionsWrapper extensionsWrapper,*/ ILanguageManager languagesManager)
        {
            this.LanguagesRepository = languagesRepository;
            this.LanguageTextsRepository = languageTextsRepository;
           // this.ExtensionsWrapper = extensionsWrapper;
            this.LanguagesManager = languagesManager;
        }

        public virtual string Add(LanguagesData languagesData)
        {
            var Languages = LanguagesRepository.FirstOrDefault(t => t.LANGUAGE_CODE == languagesData.LanguageCode);
            if (Languages != null)
            {
                throw new BlocksBussnessException("101", L("多语言编码：["+ languagesData.LanguageCode + "]已存在！"), null);
               
            }
            if (languagesData.LanguageTextsDatas.Count == 0)
            {
                throw new BlocksBussnessException("101", L("请添加语言翻译编码！"), null);

            }
            for (var i = 0; i < languagesData.LanguageTextsDatas.Count; i++)
            {
                var idx = i + 1;

                var Lkey = languagesData.LanguageTextsDatas[i].LanguageKey.TrimEnd().TrimStart();
                if (string.IsNullOrEmpty(Lkey))
                {
                    throw new BlocksBussnessException("101", L("第"+ idx + "行的翻译编码不可以为空"), null);
                }
                var vue= languagesData.LanguageTextsDatas[i].LanguageValue.TrimEnd().TrimStart();
                if (string.IsNullOrEmpty(vue))
                {
                    throw new BlocksBussnessException("101", L("第" + idx + "行的翻译内容不可以为空"), null);
                }
            }
            var key = languagesData.LanguageTextsDatas.GroupBy(x => x.LanguageKey).Where(x => x.Count() > 1).ToList();
            string keystirng = "";
            foreach (var item in key)
            {
                keystirng += item.Key + ";";

            }
            if (!string.IsNullOrEmpty(keystirng))
            {
                throw new BlocksBussnessException("101", L("翻译编码：【"+keystirng + "】重复了"), null);
            }

            var value = languagesData.LanguageTextsDatas.GroupBy(x => x.LanguageValue).Where(x => x.Count() > 1).ToList();
            string valuestring = "";
            foreach (var item in value)
            {
                valuestring += item.Key + ";";

            }
            //if (!string.IsNullOrEmpty(valuestring))
            //{
            //    throw new BlocksBussnessException("101", L("翻译内容：【" + valuestring + "】重复了"), null);
            //}

            BDTA_LANGUAGES bdtaLanguages = new BDTA_LANGUAGES();
            bdtaLanguages.Id = languagesData.Id;
            bdtaLanguages.LANGUAGE_CODE = languagesData.LanguageCode;
            bdtaLanguages.LANGUAGE_NAME = languagesData.LanguageName;
            bdtaLanguages.LANGUAGE_ICON = languagesData.LanguageIcon;

            List<BDTA_LANGUAGETEXTS> details = new List<BDTA_LANGUAGETEXTS>();
            foreach (LanguageTextsData item in languagesData.LanguageTextsDatas)
            {
                BDTA_LANGUAGETEXTS newLanguageText = new BDTA_LANGUAGETEXTS();
                newLanguageText.Id = Guid.NewGuid().ToString();
                newLanguageText.LANGUAGE_ID = languagesData.Id;
                newLanguageText.LANGUAGE_CODE = languagesData.LanguageCode;
                newLanguageText.LANGUAGE_MODULE = item.LanguageModule;
                newLanguageText.LANGUAGE_KEY = item.LanguageKey;
                newLanguageText.LANGUAGE_VALUE = item.LanguageValue;
                details.Add(newLanguageText);
            }
            var isSuccessed = LanguagesRepository.InsertAndGetId(bdtaLanguages);
            if (string.IsNullOrEmpty(isSuccessed))
            {
                throw new BlocksBussnessException("101", L("多语言主数据保存失败！"), null);
            }
            var returnDatas = LanguageTextsRepository.Insert(details);
            ///TODO:批量增加返回值的处理
            if (returnDatas.Count != details.Count)
            {
                throw new BlocksBussnessException("101", L("多语言明细保存失败！"), null);
            }
            return "新增成功";
        }

        public virtual string Update(LanguagesData languagesData)
        {
            var ltype = LanguagesRepository.FirstOrDefault(t => t.Id == languagesData.Id);
            if (ltype == null)
            {
                throw new BlocksBussnessException("101", L("请刷新界面,多语言类型可能已经被删除！"), null);
            }
            /*if (string.IsNullOrEmpty(languagesData.LanguageCode) || string.IsNullOrEmpty(languagesData.LanguageName))
            {
                throw new BlocksBussnessException("101", L("多语言编码/名称不能为空"), null);
            }
            var checkData = LanguagesRepository.FirstOrDefault(t => t.LANGUAGE_CODE == languagesData.LanguageCode);

            if (checkData != null && languagesData.Id != checkData.Id)
            {
                throw new BlocksBussnessException("101", L("多语言编码已存在"), null);
            }*/

            if (languagesData.LanguageTextsDatas.Count == 0)
            {
                throw new BlocksBussnessException("101", L("请添加语言翻译编码！"), null);

            }
            for (var i = 0; i < languagesData.LanguageTextsDatas.Count; i++)
            {
                var idx = i + 1;

                var Lkey = languagesData.LanguageTextsDatas[i].LanguageKey.TrimEnd().TrimStart();
                if (string.IsNullOrEmpty(Lkey))
                {
                    throw new BlocksBussnessException("101", L("第" + idx + "行的翻译编码不可以为空"), null);
                }
                var vue = languagesData.LanguageTextsDatas[i].LanguageValue.TrimEnd().TrimStart();
                if (string.IsNullOrEmpty(vue))
                {
                    throw new BlocksBussnessException("101", L("第" + idx + "行的翻译内容不可以为空"), null);
                }
            }
            //判断KEY值是否有重复
            // CheckRepitKey(languagesData.LanguageTextsDatas);
            //
            var key = languagesData.LanguageTextsDatas.GroupBy(x => x.LanguageKey).Where(x => x.Count() > 1).ToList();
            string keystirng = "";
            foreach (var item in key)
            {
                keystirng += item.Key + ";";

            }
            if (!string.IsNullOrEmpty(keystirng))
            {
                throw new BlocksBussnessException("101", L("翻译编码：【" + keystirng + "】重复了"), null);
            }

            var value = languagesData.LanguageTextsDatas.GroupBy(x => x.LanguageValue).Where(x => x.Count() > 1).ToList();
            string valuestring = "";
            foreach (var item in value)
            {
                valuestring += item.Key + ";";

            }
            //if (!string.IsNullOrEmpty(valuestring))
            //{
            //    throw new BlocksBussnessException("101", L("翻译内容：【" + valuestring + "】重复了"), null);
            //}
            /*  int successCount = LanguagesRepository.Update(t => t.Id == languagesData.Id, t => new BDTA_LANGUAGES()
              {
                  LANGUAGE_CODE = languagesData.LanguageCode,
                  LANGUAGE_NAME = languagesData.LanguageName,
                  LANGUAGE_ICON = languagesData.LanguageIcon
              });
              if (successCount == 0)
              {
                  throw new BlocksBussnessException("101", L("更新失败"), null);
              }*/



            LanguageTextsRepository.Delete(t => t.LANGUAGE_ID == languagesData.Id);
            List<BDTA_LANGUAGETEXTS> details = new List<BDTA_LANGUAGETEXTS>();
            foreach (LanguageTextsData item in languagesData.LanguageTextsDatas)
            {
                BDTA_LANGUAGETEXTS newLanguageText = new BDTA_LANGUAGETEXTS();
                newLanguageText.Id = Guid.NewGuid().ToString();
                newLanguageText.LANGUAGE_ID = languagesData.Id;
                newLanguageText.LANGUAGE_CODE = ltype.LANGUAGE_CODE;
                newLanguageText.LANGUAGE_MODULE = item.LanguageModule;
                newLanguageText.LANGUAGE_KEY = item.LanguageKey;
                newLanguageText.LANGUAGE_VALUE = item.LanguageValue;
                details.Add(newLanguageText);
            }
            var returnDatas = LanguageTextsRepository.Insert(details);
            ///TODO:批量增加返回值的处理
            if (returnDatas.Count != details.Count)
            {
                throw new BlocksBussnessException("101", L("多语言明细保存失败！"), null);
            }

            return "更新成功";
        }

        public virtual string Delete(Ids iDS)
        {

            LanguagesRepository.Delete(t => iDS.IDs.Contains(t.Id));
            LanguageTextsRepository.Delete(t => iDS.IDs.Contains(t.LANGUAGE_ID));

            return "删除成功！";
        }

        public virtual LanguagesData GetOneById(LanguagesData languagesData)
        {
            var languagesManage = LanguagesRepository.FirstOrDefault(t => t.Id == languagesData.Id);
            if (languagesManage == null)
            {
                throw new BlocksBussnessException("101", L("未查到此语言相关数据"), null);
            }
            var languageTexts = LanguageTextsRepository.GetAllList(t => t.LANGUAGE_ID == languagesData.Id);
            LanguagesData returnData = new LanguagesData();

            returnData.Id = languagesManage.Id;
            returnData.LanguageCode = languagesManage.LANGUAGE_CODE;
            returnData.LanguageName = languagesManage.LANGUAGE_NAME;
            returnData.LanguageIcon = languagesManage.LANGUAGE_ICON;
            List<LanguageTextsData> details = new List<LanguageTextsData>();
            languageTexts.ForEach(t =>
            {
                LanguageTextsData detail = new LanguageTextsData
                {
                    Id = t.Id,
                    LanguageId = t.LANGUAGE_ID,
                    LanguageCode = t.LANGUAGE_CODE,
                    LanguageModule = t.LANGUAGE_MODULE,
                    LanguageKey = t.LANGUAGE_KEY,
                    LanguageValue = t.LANGUAGE_VALUE,
                };
                details.Add(detail);
            });
            returnData.LanguageTextsDatas = details;
            return returnData;
        }

        public virtual PageList<LanguagesPageResult> GetPageList(LanguagesSearchModel search)
        {

            return LanguagesRepository.GetPageList(search);
        }

        public virtual PageList<ComboboxData> GetComboxList(LanguagesSearchModel search)
        {
            return LanguagesRepository.GetComboxList(search);
        }
        public virtual PageList<ComboboxData> GetComboxCodeList(LanguagesSearchModel search)
        {
            return LanguagesRepository.GetComboxCodeList(search);
        }
        

        //public virtual PageList<ComboboxData> GetModuleComboxList(LanguagesSearchModel search)
        //{
        //    var a = ExtensionsWrapper.AvailableExtensions().ToList();
            
        //    PageList<ComboboxData> details = new PageList<ComboboxData>();
        //    details.Rows = new List<ComboboxData>();
        //    details.PagerInfo = search.page;
        //    a.ForEach(t =>
        //    {
        //        ComboboxData detail = new ComboboxData
        //        {
        //            Id = t.Name,
        //            Text = t.Name
        //        };
        //        details.Rows.Add(detail);
        //    });
        //    return details;
        //}

        public virtual PageList<ComboboxData> GetLanguageComboxList(LanguagesSearchModel search)
        {
            var a = LanguagesManager.GetLanguages().ToList();
            
            PageList<ComboboxData> details = new PageList<ComboboxData>();
            details.Rows = new List<ComboboxData>();
            details.PagerInfo = search.page;
            a.ForEach(t =>
            {
                ComboboxData detail = new ComboboxData
                {
                    Id = t.Name,
                    Text = t.DisplayName
                };
                details.Rows.Add(detail);

            });
            return details;
        }

        public string CheckRepitKey(List<LanguageTextsData> languageTextsDatas)
        {
            var detailGroupBy = languageTextsDatas.GroupBy(t => t.LanguageKey).Select(t => new {
                t.Key,
                count = t.Count()
            }).ToList();
            var repeatData = detailGroupBy.FindAll(t => t.count > 1);
            if (repeatData.Count > 0)
            {
                throw new BlocksBussnessException("101", L("多语言Key值重复"), null);
            }
            return "0";
        }
    }
}
