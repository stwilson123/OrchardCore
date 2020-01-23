using System;
using System.Collections.Generic;
using BlocksCore.Application.Abstratctions.Controller.Builder;

namespace BlocksCore.Application.Abstratctions.Controller.Factory
{
    public interface IDefaultControllerBuilderFactory
    {
        /// <summary>
        /// Generates a new controller for given type.
        /// </summary>
        /// <param name="serviceName">Name of the Api controller service. For example: 'myapplication/myservice'.</param>
        /// <typeparam name="T">Type of the proxied object</typeparam>
        IDefaultControllerBuilder<T> For<T>(string servicePrefix,string serviceName);

        /// <summary>
        /// Generates multiple controllers.
        /// </summary>
        /// <typeparam name="T">Base type (class or interface) for services</typeparam>
        /// <param name="servicePrefix">Service prefix</param>
        IBatchDefaultControllerBuilder<T> ForAll<T>(string servicePrefix, IEnumerable<Type> serviceTypes);
    }
}