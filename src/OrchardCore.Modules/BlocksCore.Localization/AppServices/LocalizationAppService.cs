using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Localization.Abtractions;
using OrchardCore.Localization;

namespace BlocksCore.Localization.AppServices
{
    public class LocalizationAppService : ILocalizationAppService
    {
        private readonly ILanguageManager _languageManager;
        private readonly ILocalizationManager _localizationManager;

        public LocalizationAppService(ILanguageManager languageManager, ILocalizationManager localizationManager)
        {
            this._languageManager = languageManager;
            this._localizationManager = localizationManager;
        }

        public async Task<object> Get()
        {
            var currentLang = (await _languageManager.CurrentLanguage).Name;
            var allLanguages = (await _languageManager.GetLanguages()).Select(l => new { name = l.Name, displayName = l.DisplayName });
            var data = _localizationManager.GetDictionary(CultureInfo.GetCultureInfo(currentLang));

            return new
            {
                data,
                currentLang = currentLang,
                allLang = allLanguages
            };
        }
    }
}
