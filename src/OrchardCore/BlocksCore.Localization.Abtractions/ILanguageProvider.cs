using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlocksCore.Localization.Abtractions
{
    public interface ILanguageProvider
    {
        Task<IEnumerable<LanguageInfo>> GetLanguages();
    }
}
