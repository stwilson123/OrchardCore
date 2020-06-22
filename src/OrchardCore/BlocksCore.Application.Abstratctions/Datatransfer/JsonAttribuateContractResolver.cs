using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BlocksCore.Application.Abstratctions.Datatransfer
{
    public class JsonAttribuateContractResolver : DefaultContractResolver
    {

        protected override JsonProperty CreatePropertyFromConstructorParameter(JsonProperty matchingMemberProperty, ParameterInfo parameterInfo)
        {
            var property = base.CreatePropertyFromConstructorParameter(matchingMemberProperty, parameterInfo);
            var attributeArray = parameterInfo.GetCustomAttributes(typeof(DataTransferAttribute), false).FirstOrDefault();
            if (attributeArray != null)
            {
                property.PropertyName = ((DataTransferAttribute)attributeArray).PropertyName;
            }
            return property;
        }


        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var attributeArray = member.GetCustomAttributes(typeof(DataTransferAttribute), false).FirstOrDefault();
            if (attributeArray != null)
            {
                property.PropertyName = ((DataTransferAttribute)attributeArray).PropertyName;
            }
            return property;
        }
    }
}
