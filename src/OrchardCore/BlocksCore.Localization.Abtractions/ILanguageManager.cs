using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlocksCore.Localization.Abtractions
{
    public interface ILanguageManager
    {
        Task<LanguageInfo> CurrentLanguage { get; }

        Task<IEnumerable<LanguageInfo>> GetLanguages();
    }
}
