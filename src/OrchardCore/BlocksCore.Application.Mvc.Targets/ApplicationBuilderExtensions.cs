using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Enables multi-tenant requests support for the current path.
        /// </summary>
        public static IApplicationBuilder UseBlocksCore(this IApplicationBuilder app, Action<IApplicationBuilder> configure = null)
        {
            return app.UseOrchardCore(configure);
        }
    }
}