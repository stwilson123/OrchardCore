using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.Localization;

namespace WebApiTestModule.Providers
{
    public class TranslationProvider : ITranslationProvider
    {
        public void LoadTranslations(string cultureName, CultureDictionary dictionary)
        {
            var cultureDictionaryRecord = new List<CultureDictionaryRecord>()
            {
                new CultureDictionaryRecord("hello","WebApiTestModule",new []{"你好" })
            };

            dictionary.MergeTranslations(cultureDictionaryRecord);
        }
    }
}
