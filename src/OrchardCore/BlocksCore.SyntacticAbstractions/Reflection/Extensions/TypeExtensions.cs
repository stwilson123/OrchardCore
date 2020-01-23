using System;
using System.Reflection;
using BlocksCore.SyntacticAbstractions.Types;

namespace BlocksCore.SyntacticAbstractions.Reflection.Extensions
{
    public static class TypeExtensions
    {
        public static Assembly GetAssembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }
        
        public static object New(this Type type, params object[] args)
        {
            Check.NotNull(type, "type");
            if (args == null || args.Length == 0)
            {
                return Activator.CreateInstance(type);
            }

            return Activator.CreateInstance(type, args);
        }
    }
}