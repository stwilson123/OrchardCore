using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Extensions.DependencyInjection;
using BlocksCore.Autofac.Extensions.DependencyInjection.Paramters;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Autofac.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider,params Param[] paramters)
        {
            var container = serviceProvider.GetAutofacRoot();// as LifetimeScope;
            var paramter = paramters?.Select(p =>
            {
                if (p is NamedParam namedParameter)
                    return new NamedParameter(namedParameter.Name, namedParameter.Value);
                return null;
            });
            return container.Resolve<T>(paramter);
        }


        public static bool IsRegistered<T>(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetAutofacRoot().IsRegistered<T>();
        }
    }
}
