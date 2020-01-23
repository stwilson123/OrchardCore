using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using BlocksCore.Application.Abstratctions.Controller;
using BlocksCore.Application.Abstratctions.Manager;
using BlocksCore.SyntacticAbstractions.Types.Collections;

namespace BlocksCore.Application.Core.Manager
{
    public class DefaultControllerManager  :ControllerManger<DefaultControllerInfo<DefaultControllerActionInfo>,DefaultControllerActionInfo>
    {

        public DefaultControllerManager() : base()
        {
        }

     
        /// <summary>
        /// Searches and returns a dynamic api controller for given name.
        /// </summary>
        /// <param name="controllerName">Name of the controller</param>
        /// <returns>Controller info</returns>
        public override DefaultControllerInfo<DefaultControllerActionInfo> FindOrNull(string controllerName)
        {
            return _defaultControllers.GetOrDefault(controllerName) as DefaultControllerInfo<DefaultControllerActionInfo>;
        }

        public override IReadOnlyList<DefaultControllerInfo<DefaultControllerActionInfo>> GetAll()
        {
            return _defaultControllers.Values.OfType<DefaultControllerInfo<DefaultControllerActionInfo>>().ToImmutableList();
        }
    }

     
}