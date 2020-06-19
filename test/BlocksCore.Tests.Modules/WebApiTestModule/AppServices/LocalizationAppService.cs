using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Localization;

namespace WebApiTestModule.AppServices
{
    public class LocalizationAppService : ILocalizationAppService
    {
        private readonly IStringLocalizer T;

        public LocalizationAppService(IStringLocalizer<LocalizationAppService> stringLocalizer)
        {
            this.T = stringLocalizer;
        }
        public object GetLocalzedString()
        {
            string hello = T["hello"];
            string notFound = T["notfound"];

            return new { hello = hello, notFound = notFound };
        }
    }
}
