using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.UnitOfWork;

using BlocksCore.Data.Linq2DB.Repository; 

namespace SysMgt.BussnessRespositoryModule.ThirdSystemCall
{
    public class ThirdSystemCallLogRepository : DBSqlRepositoryBase<SYS_CALL_THIRD_SYSTEM_LOG>, IThirdSystemCallLogRepository
    {
        public ThirdSystemCallLogRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }
    }
}
