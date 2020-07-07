using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Localization.Abtractions;
using BlocksCore.Localization.Providers;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace BlocksCore.Localization
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILanguageProvider, LanguageProvider>();
        }
    }
}
