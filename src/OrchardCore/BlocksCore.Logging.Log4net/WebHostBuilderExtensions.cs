using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlocksCore.Logging
{
    public static class WebHostBuilderExtensions
    {
        internal static string DefaultConfigFileName = "log4net.config";
        public static IWebHostBuilder UseLog4Net(this IWebHostBuilder builder)
        {
            //LayoutRenderer.Register<TenantLayoutRenderer>(TenantLayoutRenderer.LayoutRendererName);

            builder.ConfigureLogging((hostingContext, logging) =>
            {
                var configFileName = $"{hostingContext.HostingEnvironment.ContentRootPath}{Path.DirectorySeparatorChar}{DefaultConfigFileName}";
                var logOptions = new Log4NetProviderOptions(configFileName);
                logging.AddLog4Net(logOptions);
            });
            //builder.ConfigureAppConfiguration((context, configuration) =>
            //{

            //    var environment = context.HostingEnvironment;
            //    environment.ConfigureNLog($"{environment.ContentRootPath}{Path.DirectorySeparatorChar}NLog.config");
            //    LogManager.Configuration.Variables["configDir"] = environment.ContentRootPath;
            //});

            return builder;
        }
    }

    // Waiting for NLog to use `IHostEnvironment`.
    //internal static class AspNetExtensions
    //{
    //    public static LoggingConfiguration ConfigureNLog(this IHostEnvironment env, string configFileRelativePath)
    //    {
    //        ConfigurationItemFactory.Default.RegisterItemsFromAssembly(typeof(AspNetExtensions).GetTypeInfo().Assembly);
    //        LogManager.AddHiddenAssembly(typeof(AspNetExtensions).GetTypeInfo().Assembly);
    //        var fileName = Path.Combine(env.ContentRootPath, configFileRelativePath);
    //        LogManager.LoadConfiguration(fileName);
    //        return LogManager.Configuration;
    //    }
    //}
}
