using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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


        [Fact]
        public void baseclass_gettype_mustbe_subclass_type()
        {
            var cls = new subClass();
            var clsType = cls.GetCurrentType();


            Assert.Equal(typeof(subClass), clsType);

            baseClass baseCls = new subClass();
            var baseClsType = baseCls.GetCurrentType();
            Assert.Equal(typeof(subClass), baseClsType);

        }

        [Fact]
        public void jsonDeserialize_PrivateProperty_Object()
        {

            var deserializedObj =  Newtonsoft.Json.JsonConvert.DeserializeObject<jsonObj>("{'str':'123'}");
            

        }

        [Fact]
        public void IEnumableToListThrowExceptionAlwaysTrigger()
        {

            IEnumerable<string> list = new List<string>() { "1"};

            IEnumerable<string> listAlwaysException = list.Where(t =>
            {
                throw new Exception();
            });

            Assert.Throws<Exception>(() => { listAlwaysException.ToList(); });
            Assert.Throws<Exception>(() => { listAlwaysException.ToList(); });
            var i = 0;
            IEnumerable<string> listSecondException = list.Where(t =>
            {
                if(i++ > 1)
                    throw new Exception();
                return t == "";
            });

            listSecondException.ToList();
            listSecondException.ToList();

        }
        [Theory]
        [InlineData(1)]
        //[InlineData(2)]

        public void Test_InitData(int i)
        {

            if (i == 2)
                throw new Exception();

        }
    }

    public class jsonObj
    {
        public string str { get; }
    }

    public abstract class baseClass
    {
        public Type GetCurrentType()
        {
            return this.GetType();
        }
    }

    public class subClass : baseClass
    {

    }
}