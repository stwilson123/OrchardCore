

using BlocksCore.Domain.Abstractions.Domain;
using SysMgt.BussnessDomainModule.SysLog;
using SysMgt.BussnessDomainModule.ThirdSystemCall;

namespace SysMgt.BussnessDomainModule.Api
{
    public interface ISystemManageApi : IDomainService
    {
        string ThirdSystemCallApply(ThirdSystemCallData pInfo);
        RtnData WriteSystemLog(SysLogData sysLogData);
    }
}
