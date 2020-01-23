using System;

namespace BlocksCore.Application.Abstratctions.Controller.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class BlocksActionNameAttribute : Attribute
    {
        public string ActionName { get; }

        public BlocksActionNameAttribute(string actionName)
        {
            ActionName = actionName;
        }
        
    }
}