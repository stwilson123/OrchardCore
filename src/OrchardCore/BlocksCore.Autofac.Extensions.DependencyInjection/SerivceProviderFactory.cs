using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Autofac.Extensions.DependencyInjection
{
    public static class SerivceProviderFactory
    {
        public static IServiceProvider CreateServiceProvider(IServiceProvider serviceProvider,IServiceCollection services,IEnumerable<ServiceDescriptor> owinSerivceDescriptor)
        {

            if (serviceProvider is AutofacServiceProvider autofacServiceProvider)
            {
                var childTag = Guid.NewGuid().ToString("N");
                var childLifetimeScope = autofacServiceProvider.LifetimeScope.BeginLifetimeScope(childTag, builder =>
                {

                    RegisterExternallyOwned(builder, owinSerivceDescriptor, childTag);
                    if(services != null)
                    builder.Populate(services.Except(owinSerivceDescriptor), childTag);
                });

                return new AutofacServiceProvider(childLifetimeScope);
            }

            IServiceProviderFactory<ContainerBuilder> serviceProviderFactory = new AutofacServiceProviderFactory();

            var containerBuilder = serviceProviderFactory.CreateBuilder(services.Except(owinSerivceDescriptor),null);
            RegisterExternallyOwned(containerBuilder, owinSerivceDescriptor,null);
            return serviceProviderFactory.CreateServiceProvider(containerBuilder);
        }
 
        private static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureLifecycle<TActivatorData, TRegistrationStyle>(
           this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder,
           ServiceLifetime lifecycleKind,
           object lifetimeScopeTagForSingleton)
        {
            switch (lifecycleKind)
            {
                case ServiceLifetime.Singleton:
                    if (lifetimeScopeTagForSingleton == null)
                    {
                        registrationBuilder.SingleInstance();
                    }
                    else
                    {
                        registrationBuilder.InstancePerMatchingLifetimeScope(lifetimeScopeTagForSingleton);
                    }

                    break;
                case ServiceLifetime.Scoped:
                    registrationBuilder.InstancePerLifetimeScope();
                    break;
                case ServiceLifetime.Transient:
                    registrationBuilder.InstancePerDependency();
                    break;
            }

            return registrationBuilder;
        }

        private static void RegisterExternallyOwned(
           ContainerBuilder builder,
           IEnumerable<ServiceDescriptor> descriptors,
           object lifetimeScopeTagForSingletons)
        {
            foreach (var descriptor in descriptors)
            {
                if (descriptor.ImplementationType != null)
                {
                    // Test if the an open generic type is being registered
                    var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
                    if (serviceTypeInfo.IsGenericTypeDefinition)
                    {
                        builder
                            .RegisterGeneric(descriptor.ImplementationType)
                            .As(descriptor.ServiceType)
                            .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                            .ExternallyOwned();
                    }
                    else
                    {
                        builder
                            .RegisterType(descriptor.ImplementationType)
                            .As(descriptor.ServiceType)
                            .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                             .ExternallyOwned();
                    }
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
                    {
                        var serviceProvider = context.Resolve<IServiceProvider>();
                        return descriptor.ImplementationFactory(serviceProvider);
                    })
                        .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                        .ExternallyOwned()
                        .CreateRegistration();
                        

                    builder.RegisterComponent(registration);
                }
                else
                {
                    builder
                        .RegisterInstance(descriptor.ImplementationInstance)
                        .As(descriptor.ServiceType)
                        .ConfigureLifecycle(descriptor.Lifetime, null)
                        .ExternallyOwned();
                }
            }
        }


        public static ContainerBuilder  CreateBuilder(this IServiceProviderFactory<ContainerBuilder> factory,IEnumerable<ServiceDescriptor> services , Action<ContainerBuilder> configurationAction)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            configurationAction?.Invoke(builder);

            return builder;
        }
    }
}
