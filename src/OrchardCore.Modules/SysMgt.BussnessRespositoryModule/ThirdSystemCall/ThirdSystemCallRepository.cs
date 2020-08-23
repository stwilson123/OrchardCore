using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;

using BlocksCore.Data.Linq2DB.Repository;
using SysMgt.BussnessDTOModule.ThirdSystemCall;
using BlocksCore.Data.Linq;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule.ThirdSystemCall
{
  

    public class ThirdSystemCallRepository : DBSqlRepositoryBase<SYS_CALL_THIRD_SYSTEM_INFO>, IThirdSystemCallRepository
    {
        public ThirdSystemCallRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        public PageList<ThirdSystemCallPageResult> GetPageList(ThirdSystemCallSearchModel search)
        {
            return GetContextTable().Paging((SYS_CALL_THIRD_SYSTEM_INFO t) => new ThirdSystemCallPageResult()
            {
                ID = t.Id,
                SystemID=t.SYSTEM_ID,
                SystemNo = t.SYSTEM_NO,
                SystemName = t.SYSTEM_NAME,
                FunctionName=t.FUNCTION_NAME,
                ParameterIn=t.PARAMETER_IN,
                ResponseValue=t.RESPONSE_VALUE,
                ExceptionMsg=t.EXCEPTION_MSG,
                ProcessTimeBegin=t.PROCESS_TIME_BEGIN,
                ProcessTimeEnd=t.PROCESS_TIME_END,
                RequestTimes=t.REQUEST_TIMES,
                ProcessResult = t.PROCESS_RESULT.ToString()
            }, search.page);
        }
    }
}
