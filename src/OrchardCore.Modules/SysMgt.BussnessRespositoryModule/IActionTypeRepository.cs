using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule;
using SysMgt.BussnessDTOModule.SysActionType;

namespace SysMgt.BussnessRespositoryModule
{
    public interface IActionTypeRepository : IRepository<SYS_ACTION_TYPE>
    {
        PageList<SysActionTypePageResult> GetPageList(SysActionTypeSeachModel search);
    }
}
