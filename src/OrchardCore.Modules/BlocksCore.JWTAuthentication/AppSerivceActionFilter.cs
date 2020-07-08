using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BlocksCore.Abstractions.Exception;
using BlocksCore.Abstractions.Security;
using BlocksCore.Users.Abstractions;
using BlocksCore.Web.Abstractions.Authentication;
using BlocksCore.Web.Abstractions.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlocksCore.JWTAuthentication
{
    class AppSerivceActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
            var isReturnToken = context.MethodInfo?.GetCustomAttribute<TokenAttribute>() != null;
            if (isReturnToken)
            {   var tokenService = context.ServiceProvider.GetService<TokenService>();
                var sourceResult = context.Result;
                if (!(sourceResult is IUser))
                    throw new BlocksException("101","Implement AuthenticationService return null IUser paramters");
                var jObject = JObject.FromObject(sourceResult);
                jObject.Add("token", JObject.FromObject(tokenService.GetToken(sourceResult as IUser)));
                context.Result = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jObject));
            }

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
