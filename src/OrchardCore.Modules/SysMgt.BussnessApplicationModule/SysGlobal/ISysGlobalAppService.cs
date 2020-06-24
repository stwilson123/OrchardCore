using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.SysGlobal;

namespace SysMgt.BussnessApplicationModule.SysGlobal
{ 
    public interface ISysGlobalAppService : IAppService
    {
        SysGlobalInfo CustomerXml(SysGlobalInfo sysGlobalInfo);

    }
}
