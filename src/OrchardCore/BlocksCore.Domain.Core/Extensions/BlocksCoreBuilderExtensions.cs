using BlocksCore.Domain.Core;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlocksCoreBuilderExtensions
    {
        /// <summary>
        /// Adds tenant level MVC services and configuration.
        /// </summary>
        public static OrchardCoreBuilder AddDomainCore(this OrchardCoreBuilder builder)
        {
         
            return builder.RegisterStartup<Startup>();
        }
    }
}
