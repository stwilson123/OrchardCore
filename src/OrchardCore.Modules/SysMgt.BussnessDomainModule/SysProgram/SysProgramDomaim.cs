using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Domain.Abstractions.Domain;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions;
using BlocksCore.Localization.Abtractions;
using SysMgt.BussnessDTOModule.SysProgram;
using SysMgt.BussnessRespositoryModule;

using SysMgt.BussnessDomainModule.Menu;
using BlocksCore.Navigation.Abstractions;
using BlocksCore.Abstractions;

namespace SysMgt.BussnessDomainModule.SysProgram
{
	public class SysProgramDomaim : IDomainService
	{
		public IUserNavigationManager NavigationManager { get; set; }
		public ISysProgramRepository SysProgramRepository { get; set; }
		public IMenuRepository MenuRepository { get; set; }
		public Localizer L { get; set; }

		public SysProgramDomaim(ISysProgramRepository sysProgramRepository, IMenuRepository menuRepository)
		{
			SysProgramRepository = sysProgramRepository;
			MenuRepository = menuRepository;
		}

		public void Bind(BindSysProgramData bindSysProgramData)
		{
            IEnumerable<NavigationItem> mainMenu;
			if (bindSysProgramData.Platform == 1)
			{
				mainMenu = NavigationManager.GetMenuAsync(Platform.Main.ToString()).Result;
			}
			else
			{
				mainMenu = NavigationManager.GetMenuAsync(Platform.Mobile.ToString()).Result;
            }
			//var mainMenu = NavigationManager.MainMenu;
			var mainMenuList = mainMenu;

			var menuInfo = MenuRepository.FirstOrDefault(t => t.Id == bindSysProgramData.ID);
			if (menuInfo == null)
			{
				throw new BlocksBussnessException("101", L("Not querying data"), null);
			}

			//SysProgramRepository.Update(t => t.PROGRAMPARENT == bindSysProgramData.ID,
			//	t => new SYS_PROGRAM() { PROGRAMPARENT = null });

			//var menus = menuInfoSRepository.FirstOrDefault(t => t.Id == bindProgramData.ID);
			//var menuP = MenuRepository.GetAllList(t => t.PID == bindSysProgramData.ID);
			//if (menuP.Count != 0)
			//{
			//	foreach (var item in menuP)
			//	{
			//		MenuRepository.Delete(t => t.PID == item.Id);
			//	}
			//}
			//MenuRepository.Delete(t => t.PID == bindSysProgramData.ID);

			long index = 0;
			var menuP = MenuRepository.GetAllList(t => t.PID == bindSysProgramData.ID && t.TYPE == "1");
			var sysProgramData = bindSysProgramData.ListsSysProgramDatas;
			foreach (var item in menuP)
			{
				var Id = item.Id;
				var delObj = sysProgramData.Where(n => n.ID == Id).FirstOrDefault();
				if (delObj == null)
				{
					sysProgramData.Remove(delObj);
					MenuRepository.Delete(n => n.Id == Id);
					var sysProgram = SysProgramRepository.FirstOrDefault(n => n.Id == Id);
					if (sysProgram != null)
					{
						sysProgram.PROGRAMPARENT = null;
						SysProgramRepository.Update(sysProgram);
					}
				}
			}
			foreach (var item in sysProgramData)
			{
				var js = 0;
				foreach (var i in menuP)
				{
					if (i.Id == item.ID)
					{
						js++;
						break;
					}
				}
				if (js != 0)
				{
					continue;
				}
				foreach (var menu in mainMenuList)
				{
					if (menu.GetUniqueId() == item.Code)
					{
						item.Name = menu.DisplayName;
						break;
					}
				}
				index++;
				List<SYS_MENUS> sYS_s = new List<SYS_MENUS>();
				SYS_MENUS sYS_ = new SYS_MENUS();
				var a = MenuRepository.GetAllList(t => t.Id == item.ID);
				if (a.Count == 0)
				{
					sYS_.Id = item.ID;
					sYS_.NAME = item.Name;
					sYS_.PID = bindSysProgramData.ID;
					sYS_.SORT = index;
					sYS_.DATAVERSION = 1;
					sYS_.ACTIVITY = 1;
					sYS_.CODE = item.Code;
					sYS_.TYPE = "1";
					sYS_.PLATFORM = bindSysProgramData.Platform;
					sYS_s.Add(sYS_);

					MenuRepository.Insert(sYS_s);
					SysProgramRepository.Update(t => t.Id == item.ID, t => new SYS_PROGRAM() { PROGRAMPARENT = bindSysProgramData.ID, PROGRAMEXTEND = index });
				}
			}
		}

		public virtual PageList<SysProgramPageResult> GetPageList(SysProgramSearchModel search)
		{
            IEnumerable<NavigationItem> menus;
			if (search.Platform == 1)
			{
				menus = NavigationManager.GetMenuAsync(Platform.Main.ToString()).Result;
			}
			else
			{
				menus = NavigationManager.GetMenuAsync(Platform.Mobile.ToString()).Result;
            }
			var list = SysProgramRepository.GetPageList(search);
			PageList<SysProgramPageResult> result = new PageList<SysProgramPageResult>
			{
				PagerInfo = list.PagerInfo,
			};
			var menuList = MenuRepository.GetAllList().Where(n => n.PLATFORM == search.Platform);
			List<SysProgramPageResult> newList = new List<SysProgramPageResult>();


            foreach(var n in list.Rows)
			{
				var hasL = menus.Where(i => i.GetUniqueId() == n.Code).FirstOrDefault();
				var name = "";
				if (hasL != null)
				{
					name = hasL.DisplayName;
				}
				else
				{
					if (n.Code != null)
					{
						name = L(n.Code);
					}
					else
					{
						name = n.Name;
					}
				}
				var code = n.Code;
				var menuObj = menuList.Where(m => m.CODE == code).FirstOrDefault();
				var icon = menuObj?.ICON;
				var sort = menuObj?.SORT;
				var desc = menuObj?.DESC;
				SysProgramPageResult obj = new SysProgramPageResult()
				{
					ID = n.ID,
					Code = n.Code,
					URL = n.URL,
					Name = name,
					Icon = icon,
					Sort = sort,
					Desc = desc
				};
				newList.Add(obj);
			};
			result.Rows = newList;
			return result;
		}
	}
}
