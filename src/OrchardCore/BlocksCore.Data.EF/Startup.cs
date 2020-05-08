using System;
using System.IO;
using System.Linq;
using BlocksCore.Abstractions.Security;
using BlocksCore.Autofac.Extensions.DependencyInjection;
using BlocksCore.Data.Abstractions;
using BlocksCore.Data.Core;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Logging;
using BlocksCore.Domain.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Environment.Extensions;
using OrchardCore.Modules;
using static Microsoft.EntityFrameworkCore.Infrastructure.EntityFrameworkServicesBuilder;

namespace BlocksCore.Data.EF
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

            System.Diagnostics.DiagnosticListener.AllListeners.Subscribe(new CommandListener());

            services.AddDataCore();
            //  services.AddSingleton(typeof(IDataBaseProviderManager),typeof(Re));
            services.AddDbContext<BlocksDbContext>(  (serviceProvider, options) =>
            {
                var dbProviderManager = serviceProvider.GetService<IDataBaseProviderManager>();
                var connection = serviceProvider.GetService<IUnitOfWork>().DbConnection;
                var currentDbProvider = dbProviderManager.GetCurrentDatabaseProvider();
                if(!(currentDbProvider is DatabaseProvider))
                {
                    throw new BlocksDataException("CurrentDbProvider is not EF DatabaseProvider.");
                }
                ((DatabaseProvider)currentDbProvider).configBuilder(options, connection);
            }, ServiceLifetime.Transient);
           
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
            RegisterRepository(services);
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

                    foreach (var defaultInterface in rep.DefaultInterface())
                    {
                        services.AddTransient(rep);
                        services.AddTransient(defaultInterface, (serviceProvider) => {
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
