using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Localization.Abtractions;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<object> Get()
        {
            var currentLang = (await _languageManager.CurrentLanguage).Name;
            var allLanguages = (await _languageManager.GetLanguages()).Select(l => new { name = l.Name, displayName = l.DisplayName });
            var data = _localizationManager.GetDictionary(CultureInfo.GetCultureInfo(currentLang))
             .Translations
             .Select(t =>
             {
                 var splits = t.Key.Split("|");
                 return new
                 {
                     ModuleName = splits != null && splits.Length > 1 ? splits.FirstOrDefault() : "Global",
                     Name = splits.LastOrDefault(),
                     Value = t.Value
                 };
             })
             .GroupBy(g => g.ModuleName)
             .Select(s => new
             {
                 name = s.Key,
                 dics = s.Select(s => new { s.Name, s.Value })
             });

            return new
            {
                data = data,
                currentLang = currentLang,
                allLang = allLanguages
            };
        }
    }
}
