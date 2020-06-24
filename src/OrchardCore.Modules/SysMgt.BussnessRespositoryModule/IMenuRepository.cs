using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Menu;

namespace SysMgt.BussnessRespositoryModule
{
   public interface IMenuRepository:IRepository<SYS_MENUS>
   {
       PageList<MenuPageResult> GetPageList(MenuSearchModel search);
   }
}
