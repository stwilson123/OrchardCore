using System.Reflection;
using BlocksCore.Application.Abstratctions.Controller;
using BlocksCore.Application.Abstratctions.Filters;
using BlocksCore.Web.Abstractions.HttpMethod;

namespace BlocksCore.WebAPI.Controllers
{
    public class MvcControllerActionInfo : DefaultControllerActionInfo
    {
        /// <summary>
        /// The HTTP verb that is used to call this action.
        /// </summary>
        public HttpVerb Verb { get; private set; }
        
        public MvcControllerActionInfo(string actionName, HttpVerb verb, MethodInfo method, IFilter[] filters = null, bool? isApiExplorerEnabled = null) : base(actionName, method, filters, isApiExplorerEnabled)
        {

            this.Verb = verb;
        }
    }
}