using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Menu;

namespace SysMgt.BussnessApplicationModule.Menu
{
	public interface IMenuAppService : IAppService
	{
		string Add(MenuInfo menuInfo);
		object Delete(MenuInfo sysRoleInfo);
		string Edit(MenuInfo menuInfo);
		// PageList<MenuPageResult> GetPageList(MenuSearchModel search);
		MenuInfo GetOneById(MenuInfo menuInfo);
		List<MenuTypeTreeInfo> GetPageList(MenuTypeSearchModel menuTypeSearchModel);

		//List<VisitMenu> GetMenu4TopN();

		void AddUserVisitStatistic(VisitMenu navigationName);

		object DropSort(MenuReqSort req);

		object DropBefore(MenuReqSort req);
	}
}
