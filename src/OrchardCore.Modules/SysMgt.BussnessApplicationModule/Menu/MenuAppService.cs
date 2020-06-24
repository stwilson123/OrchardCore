using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.AutoMapper.Abstractions;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Localization.Abtractions;
using BlocksCore.Abstractions.Security;
using SysMgt.BussnessDomainModule.Menu;
using SysMgt.BussnessDTOModule.Menu;
using BlocksCore.Navigation.Abstractions;
using BlocksCore.Abstractions;

namespace SysMgt.BussnessApplicationModule.Menu
{
	public class MenuAppService : AppService, IMenuAppService
	{
		public MenuDomain menuDomain { get; set; }
		private IUserContext userContext { get; set; }
		public Localizer L { get; set; }
		private readonly IUserNavigationManager _navigationManager;

		public MenuAppService(MenuDomain menuDomain, IUserContext userContext, IUserNavigationManager navigationManager)
		{
			this.menuDomain = menuDomain;
			this.userContext = userContext;
			_navigationManager = navigationManager;
		}
		[LocalizedDescription("MENU_ADD")]
		public string Add([LocalizedDescription("add")]MenuInfo menuInfo)
		{
			MenuData menuData = new MenuData();
			//menuData.Name = menuInfo.Name;
			menuData.Code = menuInfo.Code;
			menuData.Desc = menuInfo.Desc;
			menuData.Sort = menuInfo.Sort;
			if (!string.IsNullOrEmpty(menuInfo.PId))
			{
				menuData.pId = menuInfo.PId;
			}
			if (!string.IsNullOrEmpty(menuInfo.Icon))
			{
				menuData.Icon = menuInfo.Icon;
			}
			if (!string.IsNullOrEmpty(menuInfo.IndexIcon))
			{
				menuData.IndexIcon = menuInfo.IndexIcon;
			}
			menuData.Type = "0";
			menuData.Platform = menuInfo.Platform;
			return menuDomain.Add(menuData);
		}
		[LocalizedDescription("MENU_DELETE")]
		public object Delete([LocalizedDescription("delete")]MenuInfo sysRoleInfo)
		{
			MenuData sysRoleInfoData = new MenuData();
			sysRoleInfoData.ID = sysRoleInfo.ID;
			return menuDomain.Delete(sysRoleInfoData);
		}
		[LocalizedDescription("MENU_EDIT")]
		public string Edit([LocalizedDescription("edit")]MenuInfo menuInfo)
		{
			MenuData menuData = new MenuData();
			menuData.ID = menuInfo.ID;
			//menuData.Name = menuInfo.Name;
			menuData.Desc = menuInfo.Desc;
			menuData.Sort = menuInfo.Sort;
			if (!string.IsNullOrEmpty(menuInfo.Icon))
			{
				menuData.Icon = menuInfo.Icon;
			}
			if (!string.IsNullOrEmpty(menuInfo.IndexIcon))
			{
				menuData.IndexIcon = menuInfo.IndexIcon;
			}
			return menuDomain.Edit(menuData);
		}

		public MenuInfo GetOneById(MenuInfo menuInfo)
		{
			MenuData menuData = new MenuData
			{
				ID = menuInfo.ID
			};
			menuData = menuDomain.GetOneById(menuData);
			//menuInfo.Name = menuData.Name;
			menuInfo.Code = menuData.Code;
			menuInfo.Desc = menuData.Desc;
			menuInfo.Sort = menuData.Sort;
			menuInfo.Icon = menuData.Icon;
			menuInfo.UId = menuData.UId;
			menuInfo.IndexIcon = menuData.IndexIcon;
			return menuInfo;
		}

		public List<MenuTypeTreeInfo> GetPageList(MenuTypeSearchModel menuTypeSearchModel)
		{
			List<MenuData> list = menuDomain.GetPageList(menuTypeSearchModel.platform);
            IEnumerable<NavigationItem> menus;
			if (menuTypeSearchModel.platform == 1)
			{
				menus = _navigationManager.GetMenuAsync(Platform.Main.ToString()).Result;
			}
			else
			{
				menus = _navigationManager.GetMenuAsync(Platform.Mobile.ToString()).Result;
            }
			List<MenuTypeTreeInfo> menuInfos = new List<MenuTypeTreeInfo>();
			foreach (var item in list)
			{
				var hasL = menus.Where(n => n.GetUniqueId() == item.Code).FirstOrDefault();
				if (hasL != null)
				{
					if (hasL.IsVisible)
					{
						menuInfos.Add(new MenuTypeTreeInfo()
						{
							id = item.ID,
							pId = item.pId,
							name = hasL.DisplayName.AutoMapTo<string>()
						});
					}
				}
				else
				{
					menuInfos.Add(new MenuTypeTreeInfo()
					{
						id = item.ID,
						pId = item.pId,
						name = L(item.Code).AutoMapTo<string>()
					});
				}
			}
			return menuInfos;
		}

		/// <summary>
		/// 获取用户访问次数最多的N个菜单
		/// </summary>
		/// <returns></returns>
		//public List<VisitMenu> GetMenu4TopN()
		//{
		//	var userInfo = userContext.GetCurrentUser();
		//	return menuDomain.GetMenu4TopN(userContext.GetCurrentUser());
		//}

		public void AddUserVisitStatistic(VisitMenu navigationName)
		{
			var userInfo = userContext.GetCurrentUser();
			menuDomain.AddUserVisitStatistic(navigationName.MenuCode, userInfo.UserId);
		}

		public object DropSort(MenuReqSort req)
		{
			return menuDomain.GetDropSort(req);
		}

		public object DropBefore(MenuReqSort req)
		{
			return menuDomain.GetDropBefore(req);
		}
	}
}
