using BlocksCore.Abstractions.Extensions;
using BlocksCore.Extensions;



namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds OrchardCore services to the host service collection.
        /// </summary>
        public static OrchardCoreBuilder AddBlocksCore(this OrchardCoreBuilder builder)
        {

            builder.ConfigureServices((services) =>
            {
                services.AddSingleton<ITypeFeatureExtensionsProvider, DefaultTypeFeatureExtensionsProvider>();
            });

            return builder;
        }
    }
}
