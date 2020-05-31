using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BlocksCore.Abstractions.Exception;
using BlocksCore.SyntacticAbstractions.Collection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BlocksCore.Abstractions.DependencyInjection
{
    public static class DependencyInjectionServiceExtensions
    {
        static LazyConcurrentDictionary<IServiceCollection,NamedServiceDicionary> Types;
        public delegate TIService ServiceResolver<TIService>(string key);
        static DependencyInjectionServiceExtensions()
        {
            Types = new LazyConcurrentDictionary<IServiceCollection, NamedServiceDicionary>();
        }
        public static IServiceCollection AddSingleton(this IServiceCollection serviceCollection,string serviceKey,Type serviceType, Type implementationType)
        {
            return Add(serviceCollection, serviceKey, serviceType, implementationType, ServiceLifetime.Singleton);

        }

        public static IServiceCollection AddSingleton(this IServiceCollection serviceCollection,string serviceKey,Type implementationType)
        {
            return Add(serviceCollection, serviceKey, implementationType, implementationType, ServiceLifetime.Singleton);

        }
        
        
        public static IServiceCollection AddTransient(this IServiceCollection serviceCollection,string serviceKey, Type implementationType)
        {
            return Add(serviceCollection, serviceKey, implementationType, implementationType, ServiceLifetime.Transient);
        }
        
        public static IServiceCollection AddTransient(this IServiceCollection serviceCollection,string serviceKey,Type serviceType, Type implementationType)
        {
            return Add(serviceCollection, serviceKey, serviceType, implementationType, ServiceLifetime.Transient);
        }
        
        public static IServiceCollection AddTransient<TService,TImplement>(this IServiceCollection serviceCollection,string serviceKey)
        {
            return Add(serviceCollection, serviceKey, typeof(TService), typeof(TImplement), ServiceLifetime.Transient);
            
        }
        public static IServiceCollection AddScoped(this IServiceCollection serviceCollection,string serviceKey,Type serviceType, Type implementationType)
        {
            return Add(serviceCollection, serviceKey, serviceType, implementationType, ServiceLifetime.Scoped);

        }
        
        public static IServiceCollection AddScoped(this IServiceCollection serviceCollection,string serviceKey,Type implementationType)
        {
            return Add(serviceCollection, serviceKey, implementationType, implementationType, ServiceLifetime.Scoped);

        }
        
        private static IServiceCollection Add(IServiceCollection serviceCollection,string serviceKey,Type serviceType, Type implementationType,  ServiceLifetime lifetime)
        {
            
            serviceCollection.Add(new ServiceDescriptor(implementationType,implementationType,lifetime));

           
            var singletonNamedSerivceDic =Types.GetOrAdd(serviceCollection, (s) => new NamedServiceDicionary());

            var dicKey = new KeyValuePair<string, Type>(serviceKey, serviceType);
            singletonNamedSerivceDic.Add(dicKey, implementationType);

            serviceCollection.TryAddSingleton(singletonNamedSerivceDic);

            return serviceCollection;
            
        }
        
        public static T GetService<T>(this IServiceProvider serviceProvider,string serviceKey)
        {

            return (T)GetService(serviceProvider, serviceKey, typeof(T));
        }
        
        
        public static object GetService(this IServiceProvider serviceProvider,string serviceKey,Type serviceType)
        {
            
            
            var dicKey = new KeyValuePair<string, Type>(serviceKey,serviceType);
            
            var lastestType = serviceProvider.GetService<NamedServiceDicionary>().Get(dicKey).LastOrDefault();
            if (lastestType == null)
            {
                throw new BlocksException("101",$"Type {serviceType} is not register.");
            }

            return serviceProvider.GetService(lastestType);
        }


        public static Type GetLastNamedServiceType(this IServiceCollection serviceCollection, string serviceKey)
        {
            var singletonNamedSerivceDic = Types.GetOrAdd(serviceCollection, (s) => new NamedServiceDicionary());

            return singletonNamedSerivceDic.GetKeys().LastOrDefault(kv => kv.Key == serviceKey).Value;
        }
        
        public static bool Contians(this IServiceCollection serviceCollection,Type implementType,ServiceLifetime serviceLifetime)
        {
            return serviceCollection.Contains(new ServiceDescriptor(implementType, implementType, serviceLifetime));
        }


        public static bool Contians(this IServiceCollection serviceCollection, Type implementType)
        {
            return serviceCollection.Any(s => s.ImplementationType == implementType); 
        }
    }
        
    
}