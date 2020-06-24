using BlocksCore.Event.Abstractions;
using BlocksCore.Event.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
         /// Adds tenant level services.
         /// </summary>
         /// <param name="services"></param>
         /// <returns></returns>
        public static IServiceCollection AddEventCore(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBus>();

            return services;
        }
    }
}
