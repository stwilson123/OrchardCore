using BlocksCore.Application.Abstratctions.Datatransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.LanguageTexts
{
    public class LanguageTextsInfo 
    {
        public string Id { get; set; }
        public string LanguageModule { get; set; }
        public string LanguageId { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageKey { get; set; }
        public string LanguageValue { get; set; }
        
    }

    public class LanguagesDetailData 
    {
        public string LanguageKey { get; set; }
        public string LanguageValue { get; set; }
    }
}
