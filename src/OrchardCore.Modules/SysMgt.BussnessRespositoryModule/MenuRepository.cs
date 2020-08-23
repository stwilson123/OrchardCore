using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;

using BlocksCore.Data.Linq2DB.Repository;
using SysMgt.BussnessDTOModule.Menu;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Data.Linq;

namespace SysMgt.BussnessRespositoryModule
{
  public  class MenuRepository:DBSqlRepositoryBase<SYS_MENUS>,IMenuRepository
    {
        public  MenuRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
            
        }

        public  PageList<MenuPageResult> GetPageList(MenuSearchModel search)
        {
            return GetContextTable().Paging((SYS_MENUS sysMenus) => new MenuPageResult()
            {
                ID = sysMenus.Id,
                Name = sysMenus.NAME,
                Code = sysMenus.CODE,
                Desc = sysMenus.DESC,
                Sort = sysMenus.SORT
            }, search.page);
        }

      
    }
}
