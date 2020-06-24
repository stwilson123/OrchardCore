using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Repository;
using SysMgt.BussnessDTOModule.ThirdSystemType;
using BlocksCore.Data.EF.Linq;
using BlocksCore.Abstractions.UI.Combobox;
using System.Collections.Generic;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule.ThirdSystemType
{ 

    public class ThirdSystemTypeRepository : DBSqlRepositoryBase<SYS_THIRD_SYSTEM_TYPE>, IThirdSystemTypeRepository
    {
        public ThirdSystemTypeRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        public PageList<ThirdSystemTypePageResult> GetPageList(ThirdSystemTypeSearchModel search)
        {
            return GetContextTable().Paging((SYS_THIRD_SYSTEM_TYPE t) => new ThirdSystemTypePageResult()
            {
                ID = t.Id,
                SystemNo = t.SYSTEM_NO,
                SystemName = t.SYSTEM_NAME
            }, search.page);
        } 
    }
}
