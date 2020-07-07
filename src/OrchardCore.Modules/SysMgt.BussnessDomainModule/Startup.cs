using System;
using BlocksCore.Abstractions.Security;
using BlocksCore.Infrastructure.Abstractions.Security.Permissions;
using BlocksCore.Navigation.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Localization;
using OrchardCore.Modules;
using SysMgt.BussnessDomainModule.Languages;
using SysMgt.BussnessDomainModule.SysProgram;
using SysMgt.BussnessDomainModule.SysUserInfo;

namespace SysMgt.BussnessDomainModule
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<INavigationFilter, NavigationContructFilter>();
            services.AddTransient<IDentityUserStore, DentityUserStore>();
            services.AddTransient<IPermissionProvider, RolePermissionProvider>();
            services.AddTransient<ITranslationProvider, LanguagesLocalizationProvider>();

            

        }
    }
}
