using System;
using System.Linq;
using System.Reflection;
using BlocksCore.Application.Abstratctions.Controller;
using BlocksCore.Application.Abstratctions.Controller.Builder;
using BlocksCore.Application.Core.Controller.Builder;
using BlocksCore.SyntacticAbstractions.Reflection.Extensions;
using BlocksCore.Web.Abstractions.HttpMethod;
using BlocksCore.WebAPI.Helper;

namespace BlocksCore.WebAPI.Controllers.Builder
{
    public class MvcControllerActionBuilder<T> : DefaultControllerActionBuilder<T>
    {
        /// <summary>
        /// Selected Http verb.
        /// </summary>
        public HttpVerb? Verb { get; set; }
        
        public bool ConventionalVerbs { get; set; }

        public MvcControllerActionBuilder(IDefaultControllerBuilder<T> defaultControllerBuilder, MethodInfo methodInfo) : base(defaultControllerBuilder, methodInfo)
        {
        }

        
        /// <summary>
        /// Used to specify Http verb of the action.
        /// </summary>
        /// <param name="verb">Http very</param>
        /// <returns>Action builder</returns>
        public IDefaultControllerActionBuilder<T> WithVerb(HttpVerb verb)
        {
            Verb = verb;
            return this;
        }
         
        /// <summary>
        /// Used to add action filters to apply to this method.
        /// </summary>
        /// <param name="filters"> Action Filters to apply.</param>
        public IDefaultControllerActionBuilder<T> WithConventionalVerbs(bool conventionalVerbs)
        {
            ConventionalVerbs = conventionalVerbs;
            return this;
        }
        
        protected virtual HttpVerb GetDefaultHttpVerb()
        {
            return HttpVerb.Get;
        }


        public override DefaultControllerActionInfo GetResult()
        {
            return new MvcControllerActionInfo(
                ActionName,
                GetNormalizedVerb(ConventionalVerbs),
                Method,
                Filters,
                IsApiExplorerEnabled
            );
        }

        private HttpVerb GetNormalizedVerb(bool conventionalVerbs)
        {
            if (Verb != null)
            {
                return Verb.Value;
            }
          
            var httpMethodAttribute =
                Method.GetCustomAttributes(typeof(HttpMethodAttribute),true);

            if (httpMethodAttribute.Any())
                return ((HttpMethodAttribute) httpMethodAttribute.FirstOrDefault()).HttpMethod;

            if (conventionalVerbs)
            {
                var conventionalVerb = ApiVerbHelper.GetConventionalVerbForMethodName(ActionName);
                if (conventionalVerb == HttpVerb.Get && !HasOnlyPrimitiveIncludingNullableTypeParameters(Method))
                {
                    conventionalVerb = GetDefaultHttpVerb();
                }

                return conventionalVerb;
            }

            return GetDefaultHttpVerb();
        }
        
        private static bool HasOnlyPrimitiveIncludingNullableTypeParameters(MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().All(p => TypeHelper.IsPrimitiveExtendedIncludingNullable(p.ParameterType)); //|| p.IsDefined(typeof(FromUriAttribute)));
        }
    }
}