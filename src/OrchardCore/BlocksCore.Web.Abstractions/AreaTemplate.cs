using System;

namespace BlocksCore.Web.Abstractions
{
    public class AreaTemplate
    {

        public static string Area = "{0}/{1}";

        public static string GetAreaKey(AreaOption areaOption)
        {
            return string.Format(Area, areaOption.AreaName,areaOption.FunctionType);
        }


    }

    public class AreaOption
    {
        public string AreaName { get; set; }

        public string FunctionType { get; set; }
    }
}