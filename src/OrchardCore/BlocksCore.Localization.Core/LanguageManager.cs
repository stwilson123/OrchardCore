using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Localization.Abtractions;

namespace BlocksCore.Localization.Core
{
    public class LanguageManager : ILanguageManager
    {
        private readonly ILanguageProvider _languageProvider;

        public LanguageManager(ILanguageProvider languageProvider)
        {
            this._languageProvider = languageProvider;
        }
        public Task<LanguageInfo> CurrentLanguage => GetCurrentLanguage();

        public Task<IEnumerable<LanguageInfo>> GetLanguages()
        {
            return _languageProvider.GetLanguages();
        }

        private async Task<LanguageInfo> GetCurrentLanguage()
        {
            var languages = await _languageProvider.GetLanguages();
            

            var currentCultureName = CultureInfo.CurrentUICulture.Name;

            //Try to find exact match
            var currentLanguage = languages.FirstOrDefault(l => l.Name == currentCultureName);
            if (currentLanguage != null)
            {
                return currentLanguage;
            }

            //Try to find best match
            currentLanguage = languages.FirstOrDefault(l => currentCultureName.StartsWith(l.Name));
            if (currentLanguage != null)
            {
                return currentLanguage;
            }

            //Try to find default language
            currentLanguage = languages.FirstOrDefault(l => l.IsDefault);
            if (currentLanguage != null)
            {
                return currentLanguage;
            }

            //Get first one
            return languages.FirstOrDefault();
        }
    }
}
