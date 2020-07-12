using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Extensions;
using BlocksCore.Abstractions.Security;
using BlocksCore.Autofac.Extensions.DependencyInjection;
using BlocksCore.Data.Abstractions;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Data.EF.Repository;
using BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext;
using BlocksCore.Domain.Abstractions;
using BlocksCore.Extensions;
using BlocksCore.Security;
using LinqToDB.Configuration;
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
        public IServiceProvider ServiceProvider { get; }

        public static IList<Type> registerTypes = new List<Type>() {
                typeof(TESTENTITY), typeof(TESTENTITY2), typeof(TESTENTITY3),
                typeof(TestRepository), typeof(TestRepository3),
                typeof(TestDto) 
              //  typeof(TESTENTITYConfiguration),typeof(TESTENTITY2Configuration),typeof(TESTENTITY3Configuration)
            };
        IFeatureInfo registerFeature;
        //ShellSettings shellSettings = new ShellSettings();
        public virtual string ProviderName { get; }

        public virtual string ConnectionString { get; protected set; }

        public TestModelContext(string connectionString)
        {
            ConnectionString = connectionString;

            var builder = new OrchardCoreBuilder(Services);
            builder.AddLInq2DBDataAccess()
            .ConfigureServices(s =>
            {
                s.AddSingleton<ITypeFeatureExtensionsProvider, DefaultTypeFeatureExtensionsProvider>();
            })
            .RegisterStartup<BlocksCore.Data.Linq2DB.Sqlserver.Startup>();

            Init();
            var mockUserContext = new Mock<IUserContext>();
            mockUserContext.Setup(f => f.GetCurrentUser()).Returns(new DefaultUserIdentifier("t1","1","admin",null));
            IUserContext userContext = mockUserContext.Object;



            //var mockRegisterFeature = new Mock<IFeatureInfo>();
            //mockRegisterFeature.SetupGet(f => f.Id).Returns("TestModelContext");
            //mockRegisterFeature.SetupGet(f => f).Returns("TestModelContext");

            registerFeature = new FeatureInfo("TestModelContext", "TestModelContext",0,null,null,null,null,false);

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

            ServiceProvider =  Services.BuildServiceProvider();
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

            Services.AddTransient<LinqToDbConnectionOptions>(sp =>
            {
                var builder = new LinqToDbConnectionOptionsBuilder();
                var dbProviderManager = sp.GetService<IDataBaseProviderManager>();
                var connection = sp.GetService<IUnitOfWorkManager>().Current.DbConnection;
                var currentDbProvider = dbProviderManager.GetCurrentDatabaseProvider();
                if (!(currentDbProvider is DatabaseProvider))
                {
                    throw new BlocksDataException("CurrentDbProvider is not EF DatabaseProvider.");
                }
                builder = ((DatabaseProvider)currentDbProvider).configBuilder(builder, connection);
                return builder.Build();
            });
            Services.AddTransient<MigrateDbContext>();


            //foreach (var startup in starters)
            //{
            //    startup.Configure(null, null, serviceProvider);
            //}
            ServiceProvider = SerivceProviderFactory.CreateServiceProvider(null,Services, Services.Where(s => s.ServiceType == typeof(IHost)));
        }

        public virtual void Init()
        {

        }

        public void Dispose()
        {
           
        }
    }
}
