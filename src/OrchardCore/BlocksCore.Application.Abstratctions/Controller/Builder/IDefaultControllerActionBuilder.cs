using System.Reflection;
using BlocksCore.Application.Abstratctions.Filters;

namespace BlocksCore.Application.Abstratctions.Controller.Builder
{
    public interface IDefaultControllerActionBuilder
    {
        /// <summary>
        /// The controller builder related to this action.
        /// </summary>
        IDefaultControllerBuilder Controller { get; }

        /// <summary>
        /// Gets name of the action.
        /// </summary>
        string ActionName { get; }

        /// <summary>
        /// Gets the action method.
        /// </summary>
        MethodInfo Method { get; }



        /// <summary>
        /// Is API Explorer enabled.
        /// </summary>
        bool? IsApiExplorerEnabled { get; set; }

        /// <summary>
        /// Gets current filters.
        /// </summary>
        IFilter[] Filters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to create action for this method.
        /// </summary>
        bool DontCreate { get; set; }
    }
    
    /// <summary>
    /// This interface is used to define a dynamic api controller action.
    /// </summary>
    /// <typeparam name="T">Type of the proxied object</typeparam>
    public interface IDefaultControllerActionBuilder<T>: IDefaultControllerActionBuilder
    {


        /// <summary>
        /// Enables/Disables API Explorer for the action.
        /// </summary>
        IDefaultControllerActionBuilder<T> WithApiExplorer(bool isEnabled);

        /// <summary>
        /// Used to specify another method definition.
        /// </summary>
        /// <param name="methodName">Name of the method in proxied type</param>
        /// <returns>Action builder</returns>
        IDefaultControllerActionBuilder<T> ForMethod(string methodName);

        /// <summary>
        /// Tells builder to not create action for this method.
        /// </summary>
        /// <returns>Controller builder</returns>
        IDefaultControllerBuilder<T> DontCreateAction();

        /// <summary>
        /// Used to add action filters to apply to this action.
        /// </summary>
        /// <param name="filters"> Action Filters to apply.</param>
        IDefaultControllerActionBuilder<T> WithFilters(params IFilter[] filters);

        
            
        /// <summary>
        /// Builds the controller.
        /// This method must be called at last of the build operation.
        /// </summary>
        void Build();


        DefaultControllerActionInfo GetResult();
    }
}