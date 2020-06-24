using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Localization.Abtractions
{
    public interface ILanguageManager
    {
        LanguageInfo CurrentLanguage { get; }

        IReadOnlyList<LanguageInfo> GetLanguages();
    }
}
