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
using BlocksCore.Data.Abstractions.DataBaseProvider;
using System.Runtime.CompilerServices;
using BlocksCore.Data.Migrator.ConnectionString;

namespace BlocksCore.Data.Migrator
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds tenant level services.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMigratorCore(this IServiceCollection services, ConnectionInfo connectionInfo, Action<IMigrationRunnerBuilder, ConnectionInfo> optionbuilder)
        {


            services.AddFluentMigratorCore()
               
                 .AddScoped<IConfigureOptions<ProcessorOptions>>(
                    sp =>
                    {
                        var dyConnectionString = sp.GetRequiredService<DynamicConnectionString>();
                        return new ConfigureNamedOptions<ProcessorOptions>(
                            Options.DefaultName,
                            opt => { opt.ConnectionString = dyConnectionString.CurrentConnectionString;
                                opt.Timeout = TimeSpan.FromSeconds(30);
                            });
                    })
                .ConfigureRunner(configure =>
                {
                    if (optionbuilder != null)
                        optionbuilder(configure, connectionInfo);
                    configure.AddDataBaseProvider(connectionInfo.ProviderName);

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


        public static IServiceCollection AddMigratorSQLServer(this IServiceCollection services, ConnectionInfo connectionInfo, Action<IMigrationRunnerBuilder, ConnectionInfo> optionbuilder)
        {
            return services.AddMigratorCore(connectionInfo, optionbuilder)
                .AddScoped<DynamicConnectionString>(sp => new SQLServerConnectionString(sp.GetService<ConnectionInfo>()));
        }

        //public static IServiceCollection AddMigratorOracle(this IServiceCollection services, ConnectionInfo connectionInfo, Action<IMigrationRunnerBuilder, ConnectionInfo> optionbuilder)
        //{
        //    return services.AddMigratorCore(connectionInfo, optionbuilder)
        //        .AddScoped<DynamicConnectionString>(sp => new OracleConnectionString(sp.GetService<ConnectionInfo>()));
        //}
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
