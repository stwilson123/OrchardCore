using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BlocksCore.Domain.Abstractions.Domain;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.Localization;
using SysMgt.BussnessDTOModule.Menu;
using SysMgt.BussnessRespositoryModule;
using BlocksCore.Abstractions.Security;
using BlocksCore.Navigation.Abstractions;

namespace SysMgt.BussnessDomainModule.Menu
{
	public class MenuDomain : IDomainService
	{
		public IStringLocalizer L { get; set; }
		public IMenuRepository MenuRepository { get; set; }
		public ISysProgramRepository SysProgramRepository { get; set; }
		public IUserNavigationManager NavigationManager { get; set; }
		// public IUserIdentifier UserIdentifier { get; set; }
		public IUserVisitStatisticRepository UserVisitStatisticRepository { get; set; }

		public MenuDomain(IMenuRepository MenuRepository, ISysProgramRepository sysProgramRepository, IUserVisitStatisticRepository userVisitStatisticRepository, IUserNavigationManager navigationManager)
		{
			this.MenuRepository = MenuRepository;
			this.SysProgramRepository = sysProgramRepository;
			this.NavigationManager = navigationManager;
			// this.UserIdentifier = user;
			this.UserVisitStatisticRepository = userVisitStatisticRepository;


		}

		public string Add(MenuData menuData)
		{
			//if (menuData.Name == "")
			//{
			//	throw new BlocksBussnessException("101", L["名称不能为空!"], null);
			//}

			if (menuData.Code == "")
			{
				throw new BlocksBussnessException("101", L["编号不能为空!"], null);
			}


			//var menuInfo = MenuRepository.FirstOrDefault(t => t.NAME == menuData.Name);
			//if (menuInfo != null)
			//{
			//	throw new BlocksBussnessException("101", L["名称重复"], null);
			//}

			var menuInfo2 = MenuRepository.FirstOrDefault(t => t.CODE == menuData.Code);
			if (menuInfo2 != null)
			{
				throw new BlocksBussnessException("101", L["编号重复"], null);
			}

			SYS_MENUS sysMenus = new SYS_MENUS();
			sysMenus.NAME = menuData.Code;
			sysMenus.CODE = menuData.Code;
			sysMenus.DESC = menuData.Desc;
			sysMenus.SORT = menuData.Sort;
			sysMenus.TYPE = menuData.Type;
			if (!string.IsNullOrEmpty(menuData.pId))
			{
				sysMenus.PID = menuData.pId;
			}
			sysMenus.ICON = menuData.Icon;
			sysMenus.INDEXICON = menuData.IndexIcon;
			sysMenus.PLATFORM = menuData.Platform;
			string returnId = MenuRepository.InsertAndGetId(sysMenus);
			if (string.IsNullOrEmpty(returnId))
			{
				return "保存失败";
			}
			else
			{
				return "保存成功";
			}
		}

		public string Edit(MenuData menuData)
		{
			//if (menuData.Name == "")
			//{
			//	throw new BlocksBussnessException("101", L["名称不能为空!"], null);
			//}

			//if (menuData.Code == "")
			//{
			//	throw new BlocksBussnessException("101", L["编号不能为空!"], null);
			//}

			//var sysRoleInfo = MenuRepository.FirstOrDefault(t => t.NAME == menuData.Name && t.Id != menuData.ID);
			//if (sysRoleInfo != null)
			//{
			//	throw new BlocksBussnessException("101", L["名称重复"], null);
			//}

			//var sysRoleInfo2 = MenuRepository.FirstOrDefault(t => t.CODE == menuData.Code && t.Id != menuData.ID);
			//if (sysRoleInfo2 != null)
			//{
			//	throw new BlocksBussnessException("101", L["编号重复"], null);
			//}

			int successCount = MenuRepository.Update(t => t.Id == menuData.ID, t => new SYS_MENUS()
			{
				//NAME = menuData.Name,
				DESC = menuData.Desc,
				SORT = menuData.Sort,
				ICON = menuData.Icon,
				INDEXICON = menuData.IndexIcon
			});
			if (successCount <= 0)
			{
				return "更新失败";
			}
			else
			{
				return "更新成功";
			}
		}

		public object Delete(MenuData menuData)
		{
			var id = menuData.ID;
			//var successCount = MenuRepository.Delete(t => t.Id == id);			
			//var rows = SysProgramRepository.Delete(t => t.PROGRAMPARENT == id);
			//var menuInfos = MenuRepository.Delete(t => t.PID == id);
			//if (successCount <= 0 || rows < 0 || menuInfos < 0)
			//{
			//	throw new BlocksBussnessException("101", L["删除失败"], null);
			//}
			var childMenu = MenuRepository.GetAllList().Where(n => n.PID == id).ToList();
			if (childMenu.Count != 0)
			{
				return new
				{
					success = false,
					msg = "有子菜单不能删除"
				};
			}
			var successCount = MenuRepository.Delete(t => t.Id == id);
			var sysProgramList = SysProgramRepository.GetAllList().Where(n => n.Id == id).ToList();
			sysProgramList.ForEach(n =>
			{
				n.PROGRAMPARENT = null;
				SysProgramRepository.Update(n);
			});
			if (successCount <= 0)
			{
				return new
				{
					success = false,
					msg = "删除失败"
				};
			}
			return new
			{
				success = true,
				msg = "删除成功"
			};
		}

		public MenuData GetOneById(MenuData menuData)
		{
			var menuInfo = MenuRepository.FirstOrDefault(t => t.Id == menuData.ID);
			if (menuInfo == null)
			{
				throw new BlocksBussnessException("101", L["未找到对象"], null);
			}
			//menuData.Name = menuInfo.NAME;
			menuData.Code = menuInfo.CODE;
			menuData.Desc = menuInfo.DESC;
			menuData.Sort = menuInfo.SORT;
			menuData.Icon = menuInfo.ICON;
			menuData.IndexIcon = menuInfo.INDEXICON;
			var obj = SysProgramRepository.FirstOrDefault(n => n.PROGRAMCODE == menuInfo.CODE);
			menuData.UId = obj?.PROGRAMCODE;
			return menuData;
		}

		public List<MenuData> GetPageList(int platform = 1)
		{
			var menuInfos = MenuRepository.GetAllList()
							.Where(n => n.PLATFORM == platform)
							.OrderBy(n => n.SORT).ToList();
			List<MenuData> MenuDatas = new List<MenuData>();
			foreach (var item in menuInfos)
			{
				MenuDatas.Add(new MenuData()
				{
					ID = item.Id,
					Code = item.CODE,
					pId = item.PID
				});
			}
			return MenuDatas;
		}

		///// <summary>
		///// （首页9宫格适用）根据用户ID，获取访问次数最多的9个菜单
		///// </summary>
		///// <param name="userId"></param>
		///// <returns></returns>
		//public List<VisitMenu> GetMenu4TopN(IUserIdentifier user)
		//{
		//	if (user.RoleIds.Count() == 0)
		//	{
		//		return null;
		//	}
		//	//获取用户全部权限菜单清单
		//	var mainMenu = NavigationManager.GetMenuAsync("MainMenu").Result;
		//	List<VisitMenu> allPermissionMenuList = new List<VisitMenu>();
		//	foreach (var menu in mainMenu)
		//	{
		//		if (menu.IsVisible)
		//		{
		//			VisitMenu info = new VisitMenu();
		//			info.MenuCode = menu.GetUniqueId();
		//			info.MenuName = menu.DisplayName;
		//			info.MenuIcon = menu.Icon;
		//			allPermissionMenuList.Add(info);
		//		}
		//	}
		//	if (allPermissionMenuList == null || allPermissionMenuList.Count <= 0) //1、所有菜单现均无权限，直接返回
		//	{
		//		return null;
		//	}

		//	List<VisitMenu> vistMenuList = new List<VisitMenu>();
		//	int MaxMenuCount = 9;
		//	int visitMenuCount = 0;
		//	//获取菜单访问次数统计信息
		//	var userVisitList = UserVisitStatisticRepository.GetAllList(t => t.USER_ID == user.UserId).OrderByDescending(t => t.VISIT_NUMBERS).ThenBy(t => t.MENU_CODE);
		//	foreach (var m in userVisitList)
		//	{
		//		int tmpi = 0;
		//		foreach (var perM in allPermissionMenuList)
		//		{
		//			if (m.MENU_CODE == perM.MenuCode)
		//			{
		//				visitMenuCount++;
		//				VisitMenu v = new VisitMenu();
		//				v.MenuCode = perM.MenuCode;
		//				v.MenuName = perM.MenuName;
		//				v.MenuIcon = perM.MenuIcon;
		//				v.Seq = visitMenuCount;
		//				vistMenuList.Add(v);
		//				allPermissionMenuList.RemoveAt(tmpi); //移除已经添加的菜单  
		//				break;
		//			}
		//			tmpi++;
		//		}
		//		if (visitMenuCount >= MaxMenuCount) break;
		//	}
		//	if (visitMenuCount >= 9) //2、超过9个访问菜单统计记录，直接返回访问最多的9个菜单即返回
		//	{
		//		return GetMenuIcon(vistMenuList);
		//	}

		//	//随机补齐剩余菜单空缺 3、少于9个访问惨淡统计记录，但仍然有菜单权限的记录，补齐最多9个返回
		//	var LeftNeedMenuCount = MaxMenuCount - visitMenuCount;
		//	foreach (var perM in allPermissionMenuList)
		//	{
		//		visitMenuCount++;
		//		VisitMenu v = new VisitMenu();
		//		v.MenuCode = perM.MenuCode;
		//		v.MenuName = perM.MenuName;
		//		v.MenuIcon = perM.MenuIcon;
		//		v.Seq = visitMenuCount;
		//		vistMenuList.Add(v);
		//		if (visitMenuCount >= MaxMenuCount) break;
		//	}

		//	return GetMenuIcon(vistMenuList);
		//}

		public void AddUserVisitStatistic(string navigationName, string userId)
		{
			int successCount = UserVisitStatisticRepository.Update(t => t.USER_ID == userId && t.MENU_CODE == navigationName,
				t => new SYS_USER_VISIT_STATISTIC()
				{
					VISIT_NUMBERS = t.VISIT_NUMBERS + 1
				});
			if (successCount <= 0)
			{
				SYS_USER_VISIT_STATISTIC visit = new SYS_USER_VISIT_STATISTIC();
				visit.Id = Guid.NewGuid().ToString();
				visit.MENU_CODE = navigationName;
				visit.USER_ID = userId;
				visit.VISIT_NUMBERS = 1;
				UserVisitStatisticRepository.InsertAndGetId(visit);
			}
		}

		private List<VisitMenu> GetMenuIcon(List<VisitMenu> menuList)
		{
			var menuCodeList = menuList.Select(x => x.MenuCode).ToList();
			var menuIconList = MenuRepository.GetAllList(t => menuCodeList.Contains(t.CODE));
			foreach (var item in menuList)
			{
				foreach (var icon in menuIconList)
				{
					if (item.MenuCode == icon.CODE)
					{
						item.MenuIcon = icon.INDEXICON;
						break;
					}
				}
			}
			return menuList;
		}

		public MenuSortDTO GetDropSort(MenuReqSort req)
		{
			MenuSortDTO dto = new MenuSortDTO();
			var menu = MenuRepository.FirstOrDefault(n => n.Id == req.id);
			if (menu == null)
			{
				dto.msg = "菜单不存在";
				return dto;
			}
			var id = menu.Id;
			var pid = menu.PID;
			var menuParent = MenuRepository.FirstOrDefault(n => n.Id == req.targetId);
			if (menuParent == null)
			{
				dto.msg = "菜单不存在";
				return dto;
			}
			var parentid = menuParent.Id;
			var parentpid = menuParent.PID;
			if (string.IsNullOrEmpty(pid))
			{
				if (string.IsNullOrEmpty(parentpid))
				{
					return TodoSort(id, parentid, req.moveType);
				}
				else
				{
					dto.msg = "禁止操作";
					return dto;
				}
			}
			else
			{
				//if (!string.IsNullOrEmpty(parentpid))
				{
					if (pid == parentpid)
					{
						if (req.moveType == "inner")
						{
							if (menuParent.TYPE == "1")
							{
								dto.msg = "禁止操作";
								return dto;
							}
							else
							{
								return TodoInner(menu, menuParent);
							}
						}
						else
						{
							return TodoSort(id, parentid, req.moveType, pid);
						}
					}
					else
					{
						if (req.moveType == "next" || req.moveType == "prev")
						{
							return TodoCharge(menu, menuParent, req.moveType);
						}
						else if (req.moveType == "inner")
						{
							if (menuParent.TYPE == "1")
							{
								dto.msg = "禁止操作";
								return dto;
							}
							else
							{
								return TodoInner(menu, menuParent);
							}
						}
						else
						{
							dto.msg = "禁止操作";
							return dto;
						}
					}
				}
				//else
				//{
				//	dto.msg = "禁止操作";
				//	return dto;
				//}
			}
		}

		public MenuSortDTO GetDropBefore(MenuReqSort req)
		{
			MenuSortDTO dto = new MenuSortDTO();
			var menu = MenuRepository.FirstOrDefault(n => n.Id == req.id);
			if (menu == null)
			{
				dto.msg = "菜单不存在";
				return dto;
			}
			var id = menu.Id;
			var pid = menu.PID;
			var menuParent = MenuRepository.FirstOrDefault(n => n.Id == req.targetId);
			if (menuParent == null)
			{
				dto.msg = "菜单不存在";
				return dto;
			}
			var parentid = menuParent.Id;
			var parentpid = menuParent.PID;
			if (string.IsNullOrEmpty(pid))
			{
				if (string.IsNullOrEmpty(parentpid))
				{
					if (req.moveType == "inner")
					{
						dto.msg = "禁止操作";
						return dto;
					}
				}
				else
				{
					dto.msg = "禁止操作";
					return dto;
				}
			}
			else
			{
				if (pid == parentpid)
				{
					if (req.moveType == "inner")
					{
						if (menuParent.TYPE == "1")
						{
							dto.msg = "禁止操作";
							return dto;
						}
					}
				}
				else
				{
					if (req.moveType == "next" || req.moveType == "prev")
					{
					}
					else if (req.moveType == "inner")
					{
						if (menuParent.TYPE == "1")
						{
							dto.msg = "禁止操作";
							return dto;
						}
					}
					else
					{
						dto.msg = "禁止操作";
						return dto;
					}
				}
			}
			dto.success = true;
			return dto;
		}

		private MenuSortDTO TodoSort(string id, string targetId, string moveType, string pid = null)
		{
			MenuSortDTO dto = new MenuSortDTO();
			var menus = MenuRepository.GetAllList().Where(n => n.PID == pid).OrderBy(n => n.SORT).ToList();
			{
				var pindex = 0;
				for (var i = 0; i < menus.Count; i++)
				{
					if (menus[i].Id == targetId)
					{
						pindex = i + 1;
					}
				}
				decimal index = 0;
				if (moveType == "next")
				{
					index = (decimal)(pindex + 0.1);
				}
				else if (moveType == "prev")
				{
					index = (decimal)(pindex - 0.1);
				}
				else
				{
					dto.msg = "禁止操作";
					return dto;
				}
				var menusTemp = menus.Select(n => new MenuTempDTO()
				{
					Id = n.Id,
					Sort = n.SORT
				}).ToList();

				var thissort = menusTemp.FirstOrDefault(n => n.Id == id);
				if (thissort != null)
				{
					menusTemp.Remove(thissort);
				}

				var newobj = new MenuTempDTO()
				{
					Id = id,
					Sort = index
				};
				menusTemp.Add(newobj);
				menusTemp = menusTemp.OrderBy(n => n.Sort).ToList();
				for (var i = 0; i < menusTemp.Count; i++)
				{
					var thisid = menusTemp[i].Id;
					var obj = menus.FirstOrDefault(n => n.Id == thisid);
					if (obj != null)
					{
						obj.SORT = i + 1;
						MenuRepository.Update(obj);
					}
				}
			}
			dto.success = true;
			dto.msg = "操作成功";
			return dto;
		}

		private MenuSortDTO TodoCharge(SYS_MENUS menu, SYS_MENUS targetMenu, string moveType)
		{
			var newmenu = MenuRepository.FirstOrDefault(n => n.Id == menu.Id);
			newmenu.PID = targetMenu.PID;
			MenuRepository.Update(newmenu);
			var sysprogram = SysProgramRepository.FirstOrDefault(n => n.Id == menu.Id);
			if (sysprogram != null)
			{
				sysprogram.PROGRAMPARENT = targetMenu.PID;
				SysProgramRepository.Update(sysprogram);
			}
			return TodoSort(menu.Id, targetMenu.Id, moveType, targetMenu.PID);
		}

		private MenuSortDTO TodoInner(SYS_MENUS menu, SYS_MENUS targetMenu)
		{
			MenuSortDTO dto = new MenuSortDTO();
			var newmenu = MenuRepository.FirstOrDefault(n => n.Id == menu.Id);
			newmenu.PID = targetMenu.Id;
			newmenu.SORT = 0;
			MenuRepository.Update(newmenu);
			var sysprogram = SysProgramRepository.FirstOrDefault(n => n.Id == menu.Id);
			if (sysprogram != null)
			{
				sysprogram.PROGRAMPARENT = targetMenu.Id;
				SysProgramRepository.Update(sysprogram);
			}
			dto.success = true;
			dto.msg = "操作成功";
			return dto;
		}
	}
}