using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Extensions;
using BlocksCore.Abstractions.Security;
using BlocksCore.Autofac.Extensions.DependencyInjection;
using BlocksCore.Data.Abstractions;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Data.Core.Configurations;
using BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext;
using BlocksCore.Domain.Abstractions;
using BlocksCore.Extensions;
using BlocksCore.Security;
using LinqToDB.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Configuration;
using OrchardCore.Modules;

namespace BlocksCore.Data.Linq2DB.Test.FunctionTest.TestModel
{
    public class TestModelContext : IDisposable
    {
        public IServiceCollection Services { get; } = new ServiceCollection();
        public IServiceProvider ServiceProvider
        {
            get
            {
                if (serviceProvider == null)
                    BuildServiceProvider();
                return serviceProvider;
            }
        }
        private IServiceProvider serviceProvider;
        public static IList<Type> registerTypes = new List<Type>() {
                typeof(TESTENTITY), typeof(TESTENTITY2), typeof(TESTENTITY3),
                typeof(TestRepository),typeof(TestRepository2), typeof(TestRepository3),
              //  typeof(TestDto) 
                typeof(TESTENTITYConfiguration),typeof(TESTENTITY2Configuration),typeof(TESTENTITY3Configuration)
            };
        IFeatureInfo registerFeature;
        //ShellSettings shellSettings = new ShellSettings();
        public virtual string ProviderName { get; }

        public virtual string ConnectionString { get; protected set; }

        public TestModelContext(string connectionString)
        {
            ConnectionString = connectionString;

            var builder = new OrchardCoreBuilder(Services);
            builder.AddLinq2DBDataAccess()
            .ConfigureServices(s =>
            {
                s.AddSingleton<ITypeFeatureExtensionsProvider, DefaultTypeFeatureExtensionsProvider>();
            })
            .RegisterStartup<BlocksCore.Data.Linq2DB.Sqlserver.Startup>()
            .RegisterStartup<BlocksCore.Data.Linq2DB.Oracle.Startup>();


        }

        public void BuildServiceProvider()
        {
            Init();


            var mockUserContext = new Mock<IUserContext>();
            mockUserContext.Setup(f => f.GetCurrentUser()).Returns(new DefaultUserIdentifier("t1", "1", "admin", null));
            IUserContext userContext = mockUserContext.Object;



            //var mockRegisterFeature = new Mock<IFeatureInfo>();
            //mockRegisterFeature.SetupGet(f => f.Id).Returns("TestModelContext");
            //mockRegisterFeature.SetupGet(f => f).Returns("TestModelContext");

            registerFeature = new FeatureInfo("TestModelContext", "TestModelContext", 0, null, null, null, null, false);

            Services.AddSingleton<IClock, Clock>();
            Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddDebug();
            });

            // serviceProvider = services.BuildServiceProvider();
            var mockTypeFeatureProvider = new Mock<ITypeFeatureProvider>();
            mockTypeFeatureProvider.Setup(p => p.GetFeatureForDependency(It.Is<Type>((type) => registerTypes.Contains(type))))
                .Returns(registerFeature);


            var mockFeatureEntry = new CompiledFeatureEntry(registerFeature, registerTypes);


            var mockExtensionManager = new Mock<IExtensionManager>();
            mockExtensionManager.Setup(m => m.LoadFeaturesAsync(It.IsAny<string[]>()))
                .Returns(Task.FromResult<IEnumerable<FeatureEntry>>(new List<FeatureEntry>() {
                   mockFeatureEntry
                }));
            mockExtensionManager.Setup(m => m.LoadFeaturesAsync())
             .Returns(Task.FromResult<IEnumerable<FeatureEntry>>(new List<FeatureEntry>() {
                   mockFeatureEntry
             }));
            Services.AddSingleton<IExtensionManager>(mockExtensionManager.Object);
            Services.AddSingleton<ITypeFeatureProvider>(mockTypeFeatureProvider.Object);
            Services.AddSingleton<IUserContext>(userContext);

            serviceProvider = Services.BuildServiceProvider();
            var starters = ServiceProvider.GetServices<IStartup>().OrderBy(s => s.Order);
            foreach (var startup in starters)
            {
                startup.ConfigureServices(Services);
            }

            //Services.AddDbContext<MigrateDbContext>((serviceProvider, options) =>
            //{
            //    var dbProviderManager = serviceProvider.GetService<IDataBaseProviderManager>();
            //    var connection = serviceProvider.GetService<IUnitOfWork>().DbConnection;
            //    ((DatabaseProvider)dbProviderManager.GetCurrentDatabaseProvider()).configBuilder(options, connection);
            //}, ServiceLifetime.Transient);

            Services.AddTransient<DbContextOption<LinqToDbConnectionOptions>>(sp =>
            {
                //var builder = new LinqToDbConnectionOptionsBuilder();
                IDbContextOptionBuilder<LinqToDbConnectionOptions> builder = new DbContextOptionBuilder<LinqToDbConnectionOptions>();
                var dbProviderManager = sp.GetService<IDataBaseProviderManager>();
                var unitOfWork = sp.GetService<IUnitOfWorkManager>().Current;
                var currentDbProvider = dbProviderManager.GetCurrentDatabaseProvider();
                if (!(currentDbProvider is DatabaseProvider))
                {
                    throw new BlocksDataException("CurrentDbProvider is not EF DatabaseProvider.");
                }
                builder = ((DatabaseProvider)currentDbProvider).ConfigBuilder(builder, unitOfWork);

                return builder.Build();
            });
            Services.AddTransient<MigrateDbContext>();


            //foreach (var startup in starters)
            //{
            //    startup.Configure(null, null, serviceProvider);
            //}
            serviceProvider = SerivceProviderFactory.CreateServiceProvider(null, Services, Services.Where(s => s.ServiceType == typeof(IHost)));
        }

        public virtual void Init(bool isDatabaseCreator = false)
        {

        }

        public virtual ShellSettings CreateShellSettings()
        {
            return null;
        }
        public virtual ShellSettings CreateDatabaseCreatorSettings()
        {
            return null;
        }

        public void CloseConnection()
        {
            var dbConnection = ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Current as IDisposable;
            dbConnection.Dispose();
        }

        public void Dispose()
        {

            if (ServiceProvider is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
