using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using BlocksCore.Application.Abstratctions.Controller;
using BlocksCore.Application.Abstratctions.Manager;
using BlocksCore.SyntacticAbstractions.Types.Collections;

namespace BlocksCore.WebAPI.Controllers.Manager
{
    public class MvcControllerManager : ControllerManger<DefaultControllerInfo<MvcControllerActionInfo>,MvcControllerActionInfo>
    {
        public MvcControllerManager(): base()
        {
        }

        /// <summary>
        /// Searches and returns a dynamic api controller for given name.
        /// </summary>
        /// <param name="controllerName">Name of the controller</param>
        /// <returns>Controller info</returns>
        public override  DefaultControllerInfo<MvcControllerActionInfo> FindOrNull(string controllerName)
        {
            return _defaultControllers.GetOrDefault(controllerName) as DefaultControllerInfo<MvcControllerActionInfo>;
        }

        public override IReadOnlyList<DefaultControllerInfo<MvcControllerActionInfo>> GetAll()
        {
            return _defaultControllers.Values.OfType<DefaultControllerInfo<MvcControllerActionInfo>>().ToImmutableList();
        }
    }
}