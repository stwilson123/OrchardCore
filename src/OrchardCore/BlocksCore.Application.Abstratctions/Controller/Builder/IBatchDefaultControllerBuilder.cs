using System;
using BlocksCore.Application.Abstratctions.Filters;

namespace BlocksCore.Application.Abstratctions.Controller.Builder
{
    public interface IBatchDefaultControllerBuilder<T>
    {
        IBatchDefaultControllerBuilder<T> ForMethods(Action<IDefaultControllerActionBuilder> action);
        IBatchDefaultControllerBuilder<T> WithConventionalVerbs();
        IBatchDefaultControllerBuilder<T> WithFilters(params IFilter[] filters);
        
        /// <summary>
        /// Builds the controller.
        /// This method must be called at last of the build operation.
        /// </summary>
        void Build();
    }
}
 