using System;
using System.Linq;

namespace BlocksCore.Abstractions.Extensions
{
    /// <summary>
    /// An attribute that can associate a service or component with
    /// a specific feature by its name. This component will only
    /// be used if the feature is enabled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FeatureAttribute : OrchardCore.Modules.Manifest.FeatureAttribute, IHasChildNames
    {
        public string[] ChildNames { get; set ; } = Enumerable.Empty<string>().ToArray();
    }
}
