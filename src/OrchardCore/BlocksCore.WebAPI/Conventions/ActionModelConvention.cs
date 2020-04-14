using System.Collections.Generic;
using System.Linq;
using BlocksCore.WebAPI.Controllers;
using BlocksCore.WebAPI.Controllers.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BlocksCore.WebAPI
{
    internal class ActionModelConvention : IActionModelConvention
    {
        private MvcControllerManager _mvcControllerManager;
        public ActionModelConvention(MvcControllerManager mvcControllerManager)
        {
            _mvcControllerManager = mvcControllerManager;

        }
        public void Apply(ActionModel action)
        {
            var controllerInfo = _mvcControllerManager.GetAll()

                .FirstOrDefault(c => c.ServiceType == action.Controller.ControllerType);
            if (controllerInfo == null )
                return;

            if (controllerInfo.Actions.TryGetValue(action.ActionName, out MvcControllerActionInfo actionInfo))
            {
                var actionAttrs = actionInfo.Method.GetCustomAttributes(false);
                var addAttrs = new List<object>();
                
                //if (!action.Attributes.OfType<IActionHttpMethodProvider>().Any(c => c.HttpMethods.Any()))
                //{
                //    //default post attribute
                //    addAttrs.Add(new HttpPostAttribute());
                //}
                addAttrs.AddRange(actionAttrs);

                addAttrs.AddRange(action.Attributes.Where(actionAttr =>  typeof(IActionHttpMethodProvider).IsAssignableFrom(actionAttr.GetType())));

                if (addAttrs.Any())
                {
                    action.Selectors.Clear();
                    ConventionHelper.AddRange(action.Selectors, ConventionHelper.CreateSelectors(addAttrs));
                    // action.Selectors.Clear();
                    //ConventionHelper.AddRange(action.Selectors, ConventionHelper.CreateSelectors(controllerAttrs));
                }
            }

           // throw new System.NotImplementedException();
        }
    }
}