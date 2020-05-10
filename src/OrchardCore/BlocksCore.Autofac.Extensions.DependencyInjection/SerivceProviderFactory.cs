using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Autofac.Extensions.DependencyInjection
{
    public static class SerivceProviderFactory
    {
        public static IServiceProvider CreateServiceProvider(IServiceProvider serviceProvider,IServiceCollection services)
        {
           
            if (serviceProvider is AutofacServiceProvider autofacServiceProvider)
            {
                var childLifetimeScope = autofacServiceProvider.LifetimeScope.BeginLifetimeScope(Guid.NewGuid().ToString("N"), builder =>
                {
                    builder.Populate(services);
                });

                return new AutofacServiceProvider(childLifetimeScope);
            }

            IServiceProviderFactory<ContainerBuilder> serviceProviderFactory = new AutofacServiceProviderFactory();
            var containerBuilder = serviceProviderFactory.CreateBuilder(services);

            return serviceProviderFactory.CreateServiceProvider(containerBuilder);
        }
    }
}
