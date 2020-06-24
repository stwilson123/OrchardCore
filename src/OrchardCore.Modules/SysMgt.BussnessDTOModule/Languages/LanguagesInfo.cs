using BlocksCore.Application.Abstratctions.Datatransfer;
using SysMgt.BussnessDTOModule.LanguageTexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.Languages
{
    public class LanguagesInfo 
    {
        public string Id { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
        public string LanguageIcon { get; set; }
        public List<LanguageTextsInfo> LanguageTextsInfos { get; set; }

    }
}
