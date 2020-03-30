using System.Linq;
using BlocksCore.WebAPI.Controllers.Manager;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace BlocksCore.WebAPI
{
    public class ControllerModelConvention: IControllerModelConvention
    {
        private MvcControllerManager _mvcControllerManager;
        public ControllerModelConvention(MvcControllerManager mvcControllerManager)
        {
            _mvcControllerManager = mvcControllerManager;
        }
        public void Apply(ControllerModel controller)
        {
            var controllerInfo = _mvcControllerManager.GetAll()
                .FirstOrDefault(c => c.ServiceInterfaceType == controller.ControllerType);
            if (controllerInfo == null)
                return;
            var controllerAttrs = controllerInfo.ServiceInterfaceType.GetCustomAttributes(false);
           

            if (controllerAttrs.Any())
            {
                controller.Selectors.Clear();
                ConventionHelper.AddRange(controller.Selectors, ConventionHelper.CreateSelectors(controllerAttrs));
            }
        }
    }
}