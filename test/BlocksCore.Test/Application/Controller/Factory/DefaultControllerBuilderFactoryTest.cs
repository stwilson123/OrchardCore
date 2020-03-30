using System;
using System.Collections.Generic;
using System.Linq;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Application.Abstratctions.Controller;
using BlocksCore.Application.Abstratctions.Controller.Factory;
using BlocksCore.Application.Core.Controller.Factory;
using BlocksCore.Application.Core.Manager;
using BlocksCore.SyntacticAbstractions.Types;
using BlocksCore.Test.Application.Controller.TestModel;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlocksCore.Test.Application.Controller.Factory
{
    public class DefaultControllerBuilderFactoryTest  
    {
         private IEnumerable<Type> servicesType;
//
//        private IServiceProvider serviceProvider;

        private string ModuleName = "TestModule";
        public DefaultControllerBuilderFactoryTest()
        {
//            IServiceCollection serviceCollection = new ServiceCollection();
//            serviceCollection.AddSingleton<DefaultControllerManager>();
//            serviceCollection.AddTransient<ITestAppService, TestAppService>();
//            
//            serviceProvider = serviceCollection.BuildServiceProvider();
//
//
           servicesType = new List<Type>(){ typeof(TestAppService)  };
        }
        
        [Fact]
        public void GetAppServiceForOneWithDefault()
        {
       
            var serviceName = ModuleName + "/Test";

            var defaultControllerManager = new DefaultControllerManager();
            IDefaultControllerBuilderFactory factory = new DefaultControllerBuilderFactory(defaultControllerManager);
            factory.For<ITestAppService>(ModuleName,serviceName).WithApiExplorer(true).Build();
             
            TestAppService(serviceName,defaultControllerManager);
        }

        private void TestAppService(string serviceName,DefaultControllerManager defaultControllerManager)
        {
            var testController = defaultControllerManager.FindOrNull(serviceName);
            Assert.NotNull(testController);
            Assert.Equal(testController.ServicePrefix, ModuleName);
            Assert.Equal(testController.ServiceName, serviceName);
         //   Assert.True(testController.IsApiExplorerEnabled);
            Assert.True(testController.ApiControllerType == typeof(NopController));

            var controllerActionDefault = testController.Actions.FirstOrDefault(t => t.Key == "Default");
            Assert.NotNull(controllerActionDefault.Value);
            Assert.Equal("Default", controllerActionDefault.Value.ActionName);
       //     Assert.Equal(HttpVerb.Post, controllerActionDefault.Value.Verb);

            var controllerActionGet = testController.Actions.FirstOrDefault(t => t.Key == "TestGet");
            Assert.NotNull(controllerActionGet.Value);
            Assert.Equal("TestGet", controllerActionGet.Value.ActionName);
        //    Assert.Equal(HttpVerb.Get, controllerActionGet.Value.Verb);


            //Attribute in implement type is unavailable
            var controllerActionDelete = testController.Actions.FirstOrDefault(t => t.Key == "TestDelete");
            Assert.NotNull(controllerActionDelete.Value);
            Assert.Equal("TestDelete", controllerActionDelete.Value.ActionName);
       //     Assert.Equal(HttpVerb.Post, controllerActionDelete.Value.Verb);

            var controllerActionIgnore = testController.Actions.FirstOrDefault(t => t.Key == "TestIgnore");
            Assert.Null(controllerActionIgnore.Value);

            var controllerActionNoActionName = testController.Actions.FirstOrDefault(t => t.Key == "TestNoActionName");
            Assert.NotNull(controllerActionNoActionName.Value);
            Assert.Equal("TestNoActionName", controllerActionNoActionName.Value.ActionName);

            
        }

        [Fact]
        public void GetAllAppServiceForAll()
        {
            var servicePrefix = ModuleName;
            var serviceName = servicePrefix + "/"+ typeof(TestAppService).Name.RemovePostFix(AppService.Postfixes);
            var defaultControllerManager = new DefaultControllerManager();
            IDefaultControllerBuilderFactory factory = new DefaultControllerBuilderFactory(defaultControllerManager);
            factory.ForAll<ITestAppService>(servicePrefix,servicesType).Build();

            TestAppService(serviceName,defaultControllerManager);
        }
        
    }
}