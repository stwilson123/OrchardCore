using System;

namespace BlocksCore.WebAPI.Controllers.Builder
{
    public class MvcControllerOption
    {
        public MvcControllerOption(Type apiControllerType)
        {
            ApiControllerType = apiControllerType;
        }
        public Type ApiControllerType { get;  }
    }
}