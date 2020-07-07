using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BlocksCore.Abstractions.Datatransfer
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property, AllowMultiple = false)]
    public class DataTransferAttribute : Attribute
    {
        //TODO add addtional property to match JsonPropertyAttribute
        public DataTransferAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
        public string PropertyName { get; private set; }
    }
}
