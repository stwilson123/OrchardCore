using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BlocksCore.Test.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlocksCore.Test
{
    public class UnitTest1
    {
        [Fact]
        public void KeyvaluePairEqual()
        {
            var KeyValuePairA = new KeyValuePair<string, Type>("KeyValuePair",typeof(UnitTest1));
            var KeyValuePairB = new KeyValuePair<string, Type>("KeyValuePair",typeof(UnitTest1));

            Assert.Equal(KeyValuePairA,KeyValuePairB);
            Assert.Equal(KeyValuePairA.GetHashCode(),KeyValuePairB.GetHashCode());

            
            
            var dicKeyValues = new ConcurrentDictionary <KeyValuePair<string, Type>,string>();
            
            
            var lazyKeyValuePairA =  dicKeyValues.GetOrAdd(KeyValuePairA,(o) => "KeyValuePairA");
            var lazyKeyValuePairB =  dicKeyValues.GetOrAdd(KeyValuePairA,(o) => "KeyValuePairB");
            Assert.Equal(lazyKeyValuePairA,lazyKeyValuePairB);
            
        }

        [Fact]
        public void NullEquals_shouldbe_ThrowException()
        {

            string a = null;
            Assert.Throws<NullReferenceException>(() => { a.Equals(null); });
        }
        
        
        [Fact]
        public void serviceCollection_notregister_shouldbe_null()
        {

            IServiceCollection serviceCollection = new ServiceCollection();


            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var service =  serviceProvider.GetService<IName>();
            
            Assert.Null(service);
        }
    }
}