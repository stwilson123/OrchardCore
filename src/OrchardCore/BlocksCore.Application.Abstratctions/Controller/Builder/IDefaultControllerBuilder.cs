using System;

namespace BlocksCore.Application.Abstratctions.Controller.Builder
{
    public interface IDefaultControllerBuilder
    {
        /// <summary>
        /// Builds the controller.
        /// This method must be called at last of the build operation.
        /// </summary>
        void Build();
    }
    
    public interface IDefaultControllerBuilder<T> : IDefaultControllerBuilder
    {
        IDefaultControllerActionBuilder<T> GetMethod(string methodName);

        IDefaultControllerBuilder<T> WithApiExplorer(bool isEnabled);
        IDefaultControllerBuilder<T> ForMethods(Action<IDefaultControllerActionBuilder> action);
    }
}