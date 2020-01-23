using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using BlocksCore.Abstractions.DependencyInjection;
namespace BlocksCore.Test.DependencyInjection
{
    public class NamedService
    {
        [Fact]
        public void NamedSerivce_should_return_implementType()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IName, Name>("NamedA");

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

           
            var service =  serviceProvider.GetService<IName>("NamedA");
            
            Assert.IsType<Name>(service);


            var serviceByType = serviceProvider.GetService<Name>();
            
            Assert.IsType<Name>(serviceByType);
        }
        
        
        [Fact]
        public void NamedSerivce_should_return_last_implementType()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IName, Name>("NamedA");
            serviceCollection.AddTransient<IName, NameB>("NamedA");


            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var service =  serviceProvider.GetService<IName>("NamedA");
            
            Assert.IsType<NameB>(service);
        }
    }


    internal interface IName
    {
        
    }
    
    
    internal class Name : IName
    {
        
    }
    
    internal class NameB : IName
    {
        
    }
}