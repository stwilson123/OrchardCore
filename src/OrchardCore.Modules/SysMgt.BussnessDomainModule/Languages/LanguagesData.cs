using SysMgt.BussnessDomainModule.LanguageTexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.Languages
{
    public class LanguagesData
    {
        public string Id { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
        public string LanguageIcon { get; set; }
        public List<LanguageTextsData> LanguageTextsDatas { get; set; }

    }
}
