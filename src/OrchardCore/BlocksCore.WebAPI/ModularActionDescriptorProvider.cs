using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Application.Abstratctions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace BlocksCore.WebAPI
{
    internal class ModularActionDescriptorProvider : IActionDescriptorProvider
    {
        public int Order => -1000;

        public void OnProvidersExecuted(ActionDescriptorProviderContext context)
        {
            var result = context.Results.Where(r => typeof(IAppService).IsAssignableFrom((r as ControllerActionDescriptor).ControllerTypeInfo));
            foreach (var r in result)
            {
                if(r.ActionConstraints == null)
                    r.ActionConstraints = new List<IActionConstraintMetadata>();
                r.ActionConstraints.Add(new HttpMethodActionConstraint(new List<string>() { "POST" }));
            }

            //throw new NotImplementedException();
        }

        public void OnProvidersExecuting(ActionDescriptorProviderContext context)
        {
           // throw new NotImplementedException();
        }
    }
}
