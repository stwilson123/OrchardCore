using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Localization;

namespace BlocksCore.Localization.Abtractions
{
    public delegate LocalizedString Localizer(string text, params object[] args);
}
