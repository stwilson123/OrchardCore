using System.Linq;
using BlocksCore.Infrastructure.Abstractions.Security.Permissions;
using BlocksCore.Infrastructure.Security.Permissions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Security
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds tenant level services.
        /// </summary>
        public static IServiceCollection AddSecurityCore(this IServiceCollection services)
        {
             
            services.AddScoped<IPermissionManager, DefaultPermissionManager>();

            return services;
        }
    }
}