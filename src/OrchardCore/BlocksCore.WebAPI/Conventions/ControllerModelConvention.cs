//using System.Linq;
//using BlocksCore.SyntacticAbstractions.Types.Collections;
//using BlocksCore.WebAPI.Controllers.Manager;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ApplicationModels;

//namespace BlocksCore.WebAPI
//{
//    public class ControllerModelConvention: IControllerModelConvention
//    {
//        private MvcControllerManager _mvcControllerManager;
//        public ControllerModelConvention(MvcControllerManager mvcControllerManager)
//        {
//            _mvcControllerManager = mvcControllerManager;
//        }
//        public void Apply(ControllerModel controller)
//        {
//            var controllerInfo = _mvcControllerManager.GetAll()
//                .FirstOrDefault(c => c.ServiceType == controller.ControllerType);
//            if (controllerInfo == null)
//                return;
//            //var controllerAttrs = controllerInfo.ServiceInterfaceType.GetCustomAttributes(false);
//            //if (controllerAttrs.Any())
//            //{
//            //    // controller.Selectors.Clear();
//            //    ConventionHelper.AddRange(controller.Selectors, ConventionHelper.CreateSelectors(controllerAttrs));
//            //}
//            //Default ApiControllerAttribute
//            //controller.Filters.Add(new ApiControllerAttribute());
//            //controller.Selectors.First().EndpointMetadata.Add(new ApiControllerAttribute());
//            //if (!controller.Selectors.IsNullOrEmpty() && controller.Selectors.Any(s => s.AttributeRouteModel == null))
//            //{
//            //    controller.Selectors.First().AttributeRouteModel = new AttributeRouteModel()
//            //    {
//            //        Template = controllerInfo.ServicePrefix + "/[controller]/[action]"
//            //    };
//            //}
//            //var controllerAttrs = new object[] { new ApiControllerAttribute(),new RouteAttribute("{area:exists}/{controller}/{action}")};
//            //controller.Selectors.Clear();
            
//            //ConventionHelper.AddRange(controller.Selectors, ConventionHelper.CreateSelectors(controllerAttrs));
//            //controller.Filters.Add(new ApiControllerAttribute());
//        }
//    }
//}