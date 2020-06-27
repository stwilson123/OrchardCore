using System;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds Orchard CMS services to the application. 
        /// </summary>
        public static IServiceCollection AddBlocksCoreMS(this IServiceCollection services)
        {
            return AddBlocksCoreMS(services, null);
        }

        /// <summary>
        /// Adds Orchard CMS services to the application and let the app change the
        /// default tenant behavior and set of features through a configure action.
        /// </summary>
        public static IServiceCollection AddBlocksCoreMS(this IServiceCollection services, Action<OrchardCoreBuilder> configure)
        {
            var builder = services.AddOrchardCore()
                .AddBlocksCore()
                //.AddSitesFolder()
                // .AddCommands()
                .AddMvc()
                .AddWebAPI()
                .AddEFDataAccess()
                .AddLocalization()
                .AddEventCore()
                .AddDomainCore();
                //.AddSetupFeatures("OrchardCore.Setup")

            //.AddDataAccess()
            //.AddDataStorage()
            //.AddBackgroundService()

            //.AddTheming()
            //.AddLiquidViews()
            //.AddCaching();

            // OrchardCoreBuilder is not available in OrchardCore.ResourceManagement as it has to
            // remain independent from OrchardCore.
            //builder.ConfigureServices(s =>
            //{
            //    s.AddResourceManagement();

            //    s.AddTagHelpers<LinkTagHelper>();
            //    s.AddTagHelpers<MetaTagHelper>();
            //    s.AddTagHelpers<ResourcesTagHelper>();
            //    s.AddTagHelpers<ScriptTagHelper>();
            //    s.AddTagHelpers<StyleTagHelper>();
            //});

            configure?.Invoke(builder);

            return services;
        }
    }
}
