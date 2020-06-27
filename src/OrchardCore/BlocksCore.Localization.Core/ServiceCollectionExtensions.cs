using System;
using BlocksCore.Localization.Abtractions;
using BlocksCore.Localization.Core;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using OrchardCore.Localization;
using OrchardCore.Localization.PortableObject;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Registers the services to enable localization using Portable Object files.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        public static IServiceCollection AddLocalizationCore(this IServiceCollection services)
        {
            return AddLocalizationCore(services, null);
        }

        /// <summary>
        /// Registers the services to enable localization using Portable Object files.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="setupAction">An action to configure the Microsoft.Extensions.Localization.LocalizationOptions.</param>
        public static IServiceCollection AddLocalizationCore(this IServiceCollection services, Action<LocalizationOptions> setupAction)
        {
            services.AddPortableObjectLocalization();
            services.Replace(ServiceDescriptor.Singleton<IStringLocalizerFactory, ModularPortableObjectStringLocalizerFactory>());
            services.AddSingleton<ILanguageManager, LanguageManager>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return services;
        }
    }
}
