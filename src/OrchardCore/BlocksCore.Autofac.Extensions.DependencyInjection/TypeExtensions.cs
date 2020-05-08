using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlocksCore.Autofac.Extensions.DependencyInjection
{
    public static  class TypeExtensions
    {
        public static IEnumerable<Type> DefaultInterface(this Type implType)
        {
            return implType.GetInterfaces().Where(i => implType.Name.Contains(GetInterfaceName(i)));
        }

        private static string GetInterfaceName(Type @interface)
        {
            string name = @interface.Name;
            if (name.Length > 1 && name[0] == 'I' && char.IsUpper(name[1]))
                return name.Substring(1);
            return name;
        }
    }
}
