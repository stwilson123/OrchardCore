using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.ThirdSystemCall;
 
namespace SysMgt.BussnessApplicationModule.ThirdSystemCall
{ 
    public interface IThirdSystemCallAppService : IAppService
    {
        PageList<ThirdSystemCallPageResult> GetPageList(ThirdSystemCallSearchModel search);
        ThirdSystemCallInfo GetOneById(ThirdSystemCallInfo pInfo); 
        string Update(ThirdSystemCallInfo pInfo);
        string ReCall(CommonEntity pInfo);

    }
}
