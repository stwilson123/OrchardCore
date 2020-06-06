using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using BlocksCore.Data;
using BlocksCore.Data.EF;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Domain.Abstractions;



namespace Microsoft.Extensions.DependencyInjection
{
    public static class OrchardCoreBuilderExtensions
    {
        /// <summary>
        /// Adds tenant level data access services.
        /// </summary>
        public static OrchardCoreBuilder AddEFDataAccess(this OrchardCoreBuilder builder)
        {

            return builder.RegisterStartup<Startup>().ConfigureServices((services, serviceProvider) =>
            {
               
            });

            //builder.ConfigureServices((services,serviceProvider)=>
            //{


            //    var dbProviderManager = serviceProvider.GetService<IDataBaseProviderManager>();
            //    services.AddDbContext<BlocksDbContext>(
            //        (options) => dbProviderManager.GetCurrentDatabaseProvider().configBuilder(options, serviceProvider.GetService<IUnitOfWork>().DbConnection),
            //        ServiceLifetime.Transient);

            //    //var shellSettings = serviceProvider.GetService<ShellSettings>();

            //    //// Before the setup a 'DatabaseProvider' may be configured without a required 'ConnectionString'.
            //    //if (shellSettings.State == TenantState.Uninitialized || shellSettings["DatabaseProvider"] == null)
            //    //{
            //    //    return;
            //    //}

            //    //var currentDBProvider = serviceProvider.GetRequiredService<List<DatabaseProvider>>().FirstOrDefault(p => p.Name == shellSettings["DatabaseProvider"]);
            //    //services.AddDbContext<BlocksDbContext>(optionsAction => currentDBProvider.configBuilder(optionsAction, shellSettings["ConnectionString"]));


            //    //services.AddDbContext<EntityFrameworkCore.DbContext>(sp =>
            //    //{


            //    //    IConfiguration storeConfiguration = new YesSql.Configuration();

            //    //    switch (shellSettings["DatabaseProvider"])
            //    //    {
            //    //        case "SqlConnection":
            //    //            storeConfiguration
            //    //                .UseSqlServer(shellSettings["ConnectionString"], IsolationLevel.ReadUncommitted)
            //    //                .UseBlockIdGenerator();
            //    //            break;
            //    //        case "Sqlite":
            //    //            var shellOptions = sp.GetService<IOptions<ShellOptions>>();
            //    //            var option = shellOptions.Value;
            //    //            var databaseFolder = Path.Combine(option.ShellsApplicationDataPath, option.ShellsContainerName, shellSettings.Name);
            //    //            var databaseFile = Path.Combine(databaseFolder, "yessql.db");
            //    //            Directory.CreateDirectory(databaseFolder);
            //    //            storeConfiguration
            //    //                .UseSqLite($"Data Source={databaseFile};Cache=Shared", IsolationLevel.ReadUncommitted)
            //    //                .UseDefaultIdGenerator();
            //    //            break;
            //    //        case "MySql":
            //    //            storeConfiguration
            //    //                .UseMySql(shellSettings["ConnectionString"], IsolationLevel.ReadUncommitted)
            //    //                .UseBlockIdGenerator();
            //    //            break;
            //    //        case "Postgres":
            //    //            storeConfiguration
            //    //                .UsePostgreSql(shellSettings["ConnectionString"], IsolationLevel.ReadUncommitted)
            //    //                .UseBlockIdGenerator();
            //    //            break;
            //    //        default:
            //    //            throw new ArgumentException("Unknown database provider: " + shellSettings["DatabaseProvider"]);
            //    //    }

            //    //    if (!string.IsNullOrWhiteSpace(shellSettings["TablePrefix"]))
            //    //    {
            //    //        storeConfiguration = storeConfiguration.SetTablePrefix(shellSettings["TablePrefix"] + "_");
            //    //    }

            //    //    var store = StoreFactory.CreateAsync(storeConfiguration).GetAwaiter().GetResult();
            //    //    var indexes = sp.GetServices<IIndexProvider>();

            //    //    store.RegisterIndexes(indexes);

            //    //    return store;
            //    //});

            //    //services.AddScoped(sp =>
            //    //{
            //    //    var store = sp.GetService<IStore>();

            //    //    if (store == null)
            //    //    {
            //    //        return null;
            //    //    }

            //    //    var session = store.CreateSession();

            //    //    var scopedServices = sp.GetServices<IScopedIndexProvider>();

            //    //    session.RegisterIndexes(scopedServices.ToArray());

            //    //    ShellScope.RegisterBeforeDispose(scope =>
            //    //    {
            //    //        return session.CommitAsync();
            //    //    });

            //    //    return session;
            //    //});

            //    //services.AddTransient<IDbConnectionAccessor, DbConnectionAccessor>();
            //});

            //return builder;
        }
    }
}