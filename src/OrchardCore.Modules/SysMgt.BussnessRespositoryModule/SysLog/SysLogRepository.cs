using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;

using BlocksCore.Data.Linq2DB.Repository;
using SysMgt.BussnessDTOModule.ThirdSystemType;
using BlocksCore.Data.Linq;
using BlocksCore.Abstractions.UI.Combobox;
using System.Collections.Generic;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule.SysLog
{ 

    public class SysLogRepository : DBSqlRepositoryBase<SYS_LOG>, ISysLogRepository
    {
        public SysLogRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        //public PageList<ThirdSystemTypePageResult> GetPageList(ThirdSystemTypeSearchModel search)
        //{
        //    return GetContextTable().Paging((SYS_THIRD_SYSTEM_TYPE t) => new ThirdSystemTypePageResult()
        //    {
        //        ID = t.Id,
        //        SystemNo = t.SYSTEM_NO,
        //        SystemName = t.SYSTEM_NAME
        //    }, search.page);
        //} 
    }
}
