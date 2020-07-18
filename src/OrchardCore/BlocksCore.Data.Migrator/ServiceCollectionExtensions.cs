using System;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using BlocksCore.Data.Abstractions;
using OrchardCore.Environment.Shell;
using BlocksCore.Data.Abstractions.Infrastructure;

namespace BlocksCore.Data.Migrator
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds tenant level services.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMigratorCore(this IServiceCollection services,IServiceProvider serviceProvider ,Action<IMigrationRunnerBuilder,string> optionbuilder)
        {

            services.AddFluentMigratorCore()
                .ConfigureRunner(configure => {
                    var shellSettings = serviceProvider.GetService<ShellSettings>();
                  
                    if (optionbuilder != null)
                        optionbuilder(configure, shellSettings["ConnectionString"]);
                    configure.ScanIn(typeof(DefaultMigrator).Assembly).For.Migrations();
          
                })
               //.AddSingleton<IAssemblySourceItem>(sp => {
               //    var dbContextServices = sp.GetService<IDbContextServices>();
               //    var connectString = dbContextServices.CurrentContext.GetDbConnection().ConnectionString;
               //   // DataConnection
               //    var builder = new InnerBuilder(services);
               //    builder
               //    .WithGlobalConnectionString(connectString)
               //    .ScanIn(typeof(DefaultMigrator).Assembly).For.Migrations();
               //    if (optionbuilder != null)
               //        optionbuilder(builder, connectString );
               //    return builder.DanglingAssemblySourceItem;
               //})
                 
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            return services;
        }
    }

    class InnerBuilder : IMigrationRunnerBuilder
    {
        public InnerBuilder(IServiceCollection services)
        {
            Services = services;
            DanglingAssemblySourceItem = null;
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }

        /// <inheritdoc />
        public IAssemblySourceItem DanglingAssemblySourceItem { get; set; }
    }
}
