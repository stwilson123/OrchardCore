using System;
using System.Collections.Generic;
using BlocksCore.Application.Abstratctions.Controller;

namespace BlocksCore.Application.Abstratctions.Manager
{
    public abstract class ControllerManger<TControllerInfo,TActionInfo> : IControllerRegister
        where TControllerInfo : DefaultControllerInfo<TActionInfo>
        where TActionInfo : DefaultControllerActionInfo
    {
        protected IDictionary<string, IControllerInfo> _defaultControllers;


        public ControllerManger()
        {
            _defaultControllers  = new Dictionary<string, IControllerInfo>(StringComparer.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Searches and returns a dynamic api controller for given name.
        /// </summary>
        /// <param name="controllerName">Name of the controller</param>
        /// <returns>Controller info</returns>
        public abstract TControllerInfo FindOrNull(string controllerName);


        public abstract IReadOnlyList<DefaultControllerInfo<TActionInfo>> GetAll();
   

        public void Register(IControllerInfo controllerInfo)
        {
            _defaultControllers[controllerInfo.ServiceName] = controllerInfo;
        }
    }
}