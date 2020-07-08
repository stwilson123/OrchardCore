using BlocksCore.Navigation.Abstractions;
using BlocksCore.Navigation.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Navigation;

namespace BlocksCore.Navigation
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
         /// Adds tenant level services.
         /// </summary>
         /// <param name="services"></param>
         /// <returns></returns>
        public static IServiceCollection AddNavigationCore(this IServiceCollection services)
        {
            services.TryAddScoped<INavigationManager, NavigationManager>();
            services.TryAddSingleton<INavigationFileManager, NavigationFileManager>();
            services.TryAddSingleton<INavigationProvider, AutoRegisterNavigationProvider>();
            services.TryAddScoped<IUserNavigationManager, DefaultUserNavigationManager>();

            return services;
        }
    }
}
