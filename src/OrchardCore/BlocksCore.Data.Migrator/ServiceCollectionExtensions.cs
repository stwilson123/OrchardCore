using System;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using BlocksCore.Data.Abstractions;
using OrchardCore.Environment.Shell;
using BlocksCore.Data.Abstractions.Infrastructure;
using FluentMigrator.Runner.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FluentMigrator.Runner.Processors;

namespace BlocksCore.Data.Migrator
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds tenant level services.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMigratorCore(this IServiceCollection services, IServiceProvider serviceProvider, Action<IMigrationRunnerBuilder, string> optionbuilder)
        {


            services.AddFluentMigratorCore()
                //TODO how controll master collection string to create database??
                .AddScoped<DynamicConnectionString>()
                 .AddScoped<IConfigureOptions<ProcessorOptions>>(
                    sp =>
                    {
                        var dyConnectionString = sp.GetRequiredService<DynamicConnectionString>();
                        var shellSettings = serviceProvider.GetService<ShellSettings>();
                        return new ConfigureNamedOptions<ProcessorOptions>(
                            Options.DefaultName,
                            opt => opt.ConnectionString = (dyConnectionString?.ConnectionString == null ? shellSettings["ConnectionString"] : dyConnectionString.ConnectionString));
                    })
                .ConfigureRunner(configure =>
                {
                    var shellSettings = serviceProvider.GetService<ShellSettings>();
                    var connectionString = shellSettings["ConnectionString"];
                    if (optionbuilder != null)
                        optionbuilder(configure, shellSettings["ConnectionString"]);
                    //configure.WithGlobalConnectionString(connectionString);
                    configure.ScanIn(typeof(DefaultMigrator).Assembly).For.Migrations();
                })
                 .AddSingleton<ILoggerProvider>(sp => new SqlScriptFluentMigratorLoggerProvider(new DebuggerTextWriter(), sp.GetService<SqlScriptFluentMigratorLoggerOptions>()))
                 .Configure<SqlScriptFluentMigratorLoggerOptions>(
                opt =>
                {
                    opt.ShowSql = true;
                    opt.ShowElapsedTime = true;
                    opt.OutputGoBetweenStatements = true;
                });

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
