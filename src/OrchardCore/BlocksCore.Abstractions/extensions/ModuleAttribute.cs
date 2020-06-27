using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlocksCore.Abstractions.Extensions
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class ModuleAttribute : OrchardCore.Modules.Manifest.ModuleAttribute, IHasChildNames
    {
        public string[] ChildNames { get ; set ; } = Enumerable.Empty<string>().ToArray();
    }
}
