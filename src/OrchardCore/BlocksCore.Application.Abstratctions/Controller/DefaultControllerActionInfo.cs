using System.Reflection;
using BlocksCore.Application.Abstratctions.Filters;

namespace BlocksCore.Application.Abstratctions.Controller
{
    public class DefaultControllerActionInfo
    {
        /// <summary>
        /// Used to store an action information of a dynamic ApiController.
        /// </summary>
        /// <summary>
        /// Name of the action in the controller.
        /// </summary>
        public string ActionName { get; private set; }

        /// <summary>
        /// The method which will be invoked when this action is called.
        /// </summary>
        public MethodInfo Method { get; private set; }



        /// <summary>
        /// Dynamic Action Filters for this Controller Action.
        /// </summary>
        public IFilter[] Filters { get; set; }

        /// <summary>
        /// Is API Explorer enabled.
        /// </summary>
        public bool? IsApiExplorerEnabled { get; set; }

        /// <summary>
        /// Createa a new <see cref="DefaultControllerActionInfo"/> object.
        /// </summary>
        /// <param name="actionName">Name of the action in the controller</param>
        /// <param name="verb">The HTTP verb that is used to call this action</param>
        /// <param name="method">The method which will be invoked when this action is called</param>
        /// <param name="filters">Filters</param>
        /// <param name="isApiExplorerEnabled">Is API explorer enabled</param>
        public DefaultControllerActionInfo(
            string actionName,
            MethodInfo method,
            IFilter[] filters = null,
            bool? isApiExplorerEnabled = null)
        {
            ActionName = actionName;
            Method = method;
            IsApiExplorerEnabled = isApiExplorerEnabled;
            Filters = filters ?? new IFilter[] { }; //Assigning or initialzing the action filters.
        }
    }
}