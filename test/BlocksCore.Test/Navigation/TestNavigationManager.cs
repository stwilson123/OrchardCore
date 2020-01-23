using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Navigation.Abstractions.Manager;
using BlocksCore.Navigation.Abstractions.Provider;
using BlocksCore.Navigation.Manager;
using BlocksCore.Test.Application.Controller.TestModel;
using BlocksCore.Test.Navigation.Model;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlocksCore.Test.Navigation
{
    public class TestNavigationManager
    {
        private IServiceProvider serviceProvider;
        public TestNavigationManager()
        {

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<INavigationManager,NavigationManager>();
            serviceCollection.AddTransient<INavigationProvider, TestNavigationProvider>();
            serviceProvider = serviceCollection.BuildServiceProvider();
            
        }

        [Fact]
        public void NavigationProvider_Return_Suitable_Data()
        {
           var navigationManager = serviceProvider.GetService<INavigationManager>();
            
            navigationManager.Initialize();


            Assert.True(navigationManager.MainMenu.Items.Count(i => i.Name == "Test") == 1);
            
            Assert.True(navigationManager.MainMenu.Items.Count(i => i.Name == "Test1") == 1);

            Assert.True(navigationManager.MainMenu.Items.Count(i => i.Name == "Test2") == 1);

            Assert.True(navigationManager.MainMenu.Items.Count(i => RouteValuesEquals(i.RouteValues,new RouteValueDictionary()
            {
                { "area" ,"TestNavigationModule"},   
                { "controller" ,"controller"},   
                { "action" ,"abc"},   
            })) == 2);
            
             

        }

        private static bool RouteValuesEquals(IDictionary<string,object> dic,IDictionary<string,object> other)
        {
            if (dic == null)
                return false;
                
            if (dic.Count != other.Count)
                return false;

            foreach (var kv in dic)
            {
                if (kv.Value != dic[kv.Key])
                    return false;
            }

            return true;
        }
        

    }
}
