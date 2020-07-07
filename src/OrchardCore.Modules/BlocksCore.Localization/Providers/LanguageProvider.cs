using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Localization.Abtractions;

namespace BlocksCore.Localization.Providers
{
    public class LanguageProvider : ILanguageProvider
    {
        public Task<IEnumerable<LanguageInfo>> GetLanguages()
        {
            IEnumerable<LanguageInfo> languages = new List<LanguageInfo>() {
                new LanguageInfo("en-US","English"),
                new LanguageInfo("zh-CN","中文简体"),

                 };
            return Task.FromResult(languages);
        }
    }
}
