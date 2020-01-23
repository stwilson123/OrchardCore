using System;
using BlocksCore.Test.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlocksCore.Test.Navigation.Model
{
    public class TestScope
    {
        [Fact]
        public void Scope_instance_should_differentparent()
        {
            
            
            
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IName, Name>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var service =  serviceProvider.GetService<IName>();

            var serviceB = serviceProvider.CreateScope().ServiceProvider.GetService<IName>();
            
            Assert.Same(service, serviceB);
        }
    }
}