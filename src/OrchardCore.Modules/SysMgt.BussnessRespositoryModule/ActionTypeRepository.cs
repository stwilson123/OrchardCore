using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;

using BlocksCore.Data.Linq2DB.Repository;
using SysMgt.BussnessDTOModule;
using SysMgt.BussnessDTOModule.SysActionType;
using BlocksCore.Data.Linq;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule
{
   public class ActionTypeRepository: DBSqlRepositoryBase<SYS_ACTION_TYPE>, IActionTypeRepository
    {
        public ActionTypeRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        public PageList<SysActionTypePageResult> GetPageList(SysActionTypeSeachModel search)
        {
            return GetContextTable().Paging((SYS_ACTION_TYPE t) => new SysActionTypePageResult()
            {
                ID = t.Id,
                TypeCode = t.TYPE_CODE,
                TypeName = t.TYPE_NAME
            }, search.page);
        }
    }
}
