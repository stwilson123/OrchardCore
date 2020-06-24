using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.SysRoleInfo;

namespace SysMgt.BussnessRespositoryModule
{
   public interface ISysRoleInfoRepository:IRepository<SYS_ROLEINFO>
   {
       PageList<SysRoleInfoPageResult> GetPageList(SysRoleInfoSearchModel search);
        PageList<SysRoleInfoPageResult> GetUserAuList(SysRoleInfoSearchModel search);
        PageList<ComboboxData> GetComboxList(SearchModel search);
   }
}
