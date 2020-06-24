using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.ThirdSystemCall;
 

namespace SysMgt.BussnessRespositoryModule.ThirdSystemCall
{
 
    public interface IThirdSystemCallRepository : IRepository<SYS_CALL_THIRD_SYSTEM_INFO>
    {
        PageList<ThirdSystemCallPageResult> GetPageList(ThirdSystemCallSearchModel search);
    }
}
