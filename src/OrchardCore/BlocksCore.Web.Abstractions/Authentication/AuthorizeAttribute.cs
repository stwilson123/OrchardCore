//using System;
//using System.Buffers.Binary;
//using System.Collections.Generic;
//using System.Text;

//namespace BlocksCore.Web.Abstractions.Authentication
//{
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
//    public class AuthorizeAttribute : Attribute
//    {
//        //
//        // 摘要:
//        //     Initializes a new instance of the Microsoft.AspNetCore.Authorization.AuthorizeAttribute
//        //     class.
//        public AuthorizeAttribute()
//        {
//            this.IsAuthorize = true;
//        }
//        //
//        // 摘要:
//        //     Initializes a new instance of the Microsoft.AspNetCore.Authorization.AuthorizeAttribute
//        //     class with the specified policy.
//        //
//        // 参数:
//        //   policy:
//        //     The name of the policy to require for authorization.
//        public AuthorizeAttribute(string policy) : this()
//        {
//            this.Policy = policy;
//        }

//        public AuthorizeAttribute(bool isAuthorize)
//        {
//            IsAuthorize = isAuthorize;
//        }
//        //
//        // 摘要:
//        //     Gets or sets a comma delimited list of schemes from which user information is
//        //     constructed.
//        public string AuthenticationSchemes { get; set; }
//        //
//        // 摘要:
//        //     Gets or sets the policy name that determines access to the resource.
//        public string Policy { get; set; }
//        //
//        // 摘要:
//        //     Gets or sets a comma delimited list of roles that are allowed to access the resource.
//        public string Roles { get; set; }
//        public bool IsAuthorize { get; }
//    }
//}
