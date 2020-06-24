using System;

namespace BlocksCore.Localization.Abtractions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
    public class LocalizedDescriptionAttribute : Attribute
    {
        public LocalizedDescriptionAttribute(string name)
        {
        }
    }
}
