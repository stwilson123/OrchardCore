using System;
using BlocksCore.Application.Abstratctions.Filters;

namespace BlocksCore.Application.Abstratctions.Controller
{
    public interface IControllerInfo
    {
        /// <summary>
        /// Name of the service.
        /// </summary>
        string ServiceName { get;  }

        /// <summary>
        /// Service interface type.
        /// </summary>
        Type ServiceType { get;   }

        /// <summary>
        /// Api Controller type.
        /// </summary>
        Type ApiControllerType { get;   }


        Type ServiceInterfaceType { get; }

        /// <summary>
        /// Interceptor type.
        /// </summary>
        Type InterceptorType { get;  }

        /// <summary>
        /// Is API Explorer enabled.
        /// </summary>
        bool? IsApiExplorerEnabled { get;  }

        /// <summary>
        /// Dynamic Attribute for this controller.
        /// </summary>
        IFilter[] Filters { get; set; }
    }
}