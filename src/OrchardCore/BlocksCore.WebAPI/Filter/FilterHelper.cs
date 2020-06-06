using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlocksCore.WebAPI.Filter
{
     class FilterHelper
    {
        public static ObjectHandleResult IsObjectResult(IActionResult objectResult, ActionDescriptor actionDescriptor)
        {
            object resultObj;
            bool isObjectResult = false;
            if (objectResult is ObjectResult)
            {
                isObjectResult = true;
                resultObj = ((ObjectResult)objectResult).Value;
            }
            else if (actionDescriptor is ControllerActionDescriptor controllerActionDescriptor && !typeof(IActionResult).IsAssignableFrom(controllerActionDescriptor.MethodInfo.ReturnType))
            {
                isObjectResult = true;
                resultObj = null;
            }
            else
            {
                resultObj = objectResult;

            }

            return new ObjectHandleResult { IsObjectResult = isObjectResult, Result = resultObj };
        }
    }

    class ObjectHandleResult
    {
       public bool IsObjectResult { set; get; }
       public object Result { set; get; }
    }
}
