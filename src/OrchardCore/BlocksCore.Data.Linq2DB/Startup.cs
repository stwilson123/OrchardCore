using System;
using System.IO;
using System.Linq;
using BlocksCore.Abstractions.Security;
using BlocksCore.Data.Abstractions;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Data.Core;
using BlocksCore.Data.Linq2DB.DBContext;
using BlocksCore.Domain.Abstractions;
using LinqToDB.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;
using OrchardCore.Modules;
using BlocksCore.Autofac.Extensions.DependencyInjection;
using FluentMigrator;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Core.Configurations;
using System.Data.Common;
using LinqToDB.Data;
using System.Diagnostics;

namespace BlocksCore.Data.Linq2DB
{
    public class Startup : StartupBase
    {
        public override int Order => -1000;
        public override int ConfigureOrder => 1000;

        private readonly IServiceProvider _serviceProvider;

        private readonly IExtensionManager _extensionManager;

        public Startup(IServiceProvider serviceProvider, IExtensionManager extensionManager)
        {
            _serviceProvider = serviceProvider;
            _extensionManager = extensionManager;
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
             

        }

        public override void ConfigureServices(IServiceCollection services)
        {


            services.AddDataCore();
            //  services.AddSingleton(typeof(IDataBaseProviderManager),typeof(Re));

            services.TryAddTransient<DbContextOption<LinqToDbConnectionOptions>>((serviceProvider) =>
            {
                //var builder = new LinqToDbConnectionOptionsBuilder();
                IDbContextOptionBuilder<LinqToDbConnectionOptions> builder = new DbContextOptionBuilder<LinqToDbConnectionOptions>();
                var dbProviderManager = serviceProvider.GetService<IDataBaseProviderManager>();
                var unitOfWork = serviceProvider.GetService<IUnitOfWorkManager>().Current;
                var currentDbProvider = dbProviderManager.GetCurrentDatabaseProvider();
                if (!(currentDbProvider is DatabaseProvider))
                {
                    throw new BlocksDataException("CurrentDbProvider is not EF DatabaseProvider.");
                }
                builder = ((DatabaseProvider)currentDbProvider).ConfigBuilder(builder, unitOfWork);
                return builder.Build();
            });
            services.TryAddTransient<BlocksDbContext>();

            services.TryAddTransient<IUnitOfWork, Linq2DbUnitOfWork>();
            RegisterRepository(services);


            DataConnection.TurnTraceSwitchOn();
            DataConnection.WriteTraceLine = (s1, s2, level) => Debug.WriteLine(s1, s2);
            LinqToDB.Common.Configuration.Linq.GenerateExpressionTest = true;
           // LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
        }

        void RegisterRepository(IServiceCollection services)
        {

            foreach (var featureEntry in _extensionManager.LoadFeaturesAsync().Result)
            {
                var repositories = featureEntry.ExportedTypes.Where(typeInfo =>
                     !typeInfo.IsAbstract &&
                           typeInfo.IsClass && typeof(IRepository).IsAssignableFrom(typeInfo)
                  );
                foreach (var rep in repositories)
                {
                    services.AddTransient(rep);
                    foreach (var defaultInterface in rep.DefaultInterface())
                    {
                        services.AddTransient(defaultInterface, (serviceProvider) =>
                        {
                            var repObj = serviceProvider.GetService(rep);
                            rep.GetProperties().Where(p => p.PropertyType == typeof(IClock)).FirstOrDefault()?.SetValue(repObj, serviceProvider.GetService<IClock>());
                            rep.GetProperties().Where(p => p.PropertyType == typeof(IUserContext)).FirstOrDefault()?.SetValue(repObj, serviceProvider.GetService<IUserContext>());
                            return repObj;
                        });
                    }
                }
            }
        }



    }

}
