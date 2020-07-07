using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Authentication
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TokenAttribute : Attribute
    {
    }
}
