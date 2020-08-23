using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;

using BlocksCore.Data.Linq2DB.Repository;
using SysMgt.BussnessDTOModule.Setup;
using BlocksCore.Data.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule
{
    public class SetupTypeRepository : DBSqlRepositoryBase<BDTA_SETUP_TYPE>, ISetupTypeRepository
    {
        public SetupTypeRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {

        }
        public PageList<SetupTypePageResult> GetPageList(SetupTypeSearchModel search)
        {
            var contextTable = GetContextTable();
            return contextTable.Paging((BDTA_SETUP_TYPE t) => new SetupTypePageResult
            {
                ID = t.Id,
                SetupTypeNo = t.SETUP_TYPE_NO,
                SetupTypeName = t.SETUP_TYPE_NAME,
                SetupTypeValue = t.SETUP_TYPE_VALUE
            }, search.page);
        }
    }
}
