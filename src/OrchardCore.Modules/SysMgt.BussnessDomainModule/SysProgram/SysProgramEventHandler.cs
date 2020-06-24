//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Blocks.BussnessEntityModule;
//using Blocks.Core.Navigation.Models;
//using Blocks.Framework.Event;
//using BlocksCore.Localization.Abtractions;
//using Blocks.LayoutModule.ExtensionsModule.Event;
//using Blocks.Framework.Navigation;
//using SysMgt.BussnessRespositoryModule;
//using BlocksCore.Abstractions.Security;


//namespace SysMgt.BussnessDomainModule.SysProgram
//{


//	public class SysProgramEventHandler : IDomainEventHandler<MenusSortEventData>
//	{
//		public Localizer L { get; set; }
//		public ISysProgramRepository SysProgramRepository { get; set; }
//		public IMenuRepository MenuRepository { get; set; }
//		private IUserContext UserContext { get; set; }
//		public ISysRoleUserRepository SysRoleUserRepository { get; set; }
//		public ISysRoleAuthorizeRespository SysRoleAuthorizeRespository { get; set; }

//		public SysProgramEventHandler(ISysProgramRepository SysProgramRepository, IMenuRepository MenuRepository, ISysRoleUserRepository SysRoleUserRepository, ISysRoleAuthorizeRespository SysRoleAuthorizeRespository, IUserContext UserContext)
//		{
//			this.SysProgramRepository = SysProgramRepository;
//			this.MenuRepository = MenuRepository;
//			this.UserContext = UserContext;
//			this.SysRoleUserRepository = SysRoleUserRepository;
//			this.SysRoleAuthorizeRespository = SysRoleAuthorizeRespository;
//		}

//		public void HandleEvent(MenusSortEventData eventData)
//		{
//			var userMenus = eventData.userNavigation.Items;
//			var sysPrograms = SysProgramRepository.GetAllList();
//			#region 注释
//			//List<SYS_PROGRAM> sysProgramList = new List<SYS_PROGRAM>();
//			//foreach (var item in items)
//			//{
//			//    if (string.IsNullOrEmpty(item.Url))
//			//    {
//			//        return;
//			//    }

//			//    //查询数据库中是否存在改菜单，如果存在更新信息，否则新增
//			//    var sysProgramInfo = sysPrograms.FirstOrDefault(t => t.PROGRAMCODE == item.GetUniqueId());
//			//    if (sysProgramInfo == null)
//			//    {
//			//        SYS_PROGRAM sysProgram = new SYS_PROGRAM();
//			//        sysProgram.Id = Guid.NewGuid().ToString();
//			//        sysProgram.PROGRAMCODE = item.GetUniqueId();
//			//        sysProgram.PROGRAMNAME = item.DisplayName.ToString();
//			//        sysProgram.PROGRAMPROPERTY = item.Url;
//			//        sysProgram.ACTIVITY = 0;
//			//        sysProgramList.Add(sysProgram);
//			//    }
//			//    else if(sysProgramInfo.PROGRAMCODE != item.GetUniqueId() || sysProgramInfo.PROGRAMNAME != item.DisplayName.ToString()
//			//        || sysProgramInfo.PROGRAMPROPERTY != item.Url)
//			//    {
//			//        SysProgramRepository.Update(t => t.Id == sysProgramInfo.Id, t => new SYS_PROGRAM()
//			//        {
//			//            PROGRAMCODE = item.GetUniqueId(),
//			//            PROGRAMNAME = item.DisplayName.ToString(),
//			//            PROGRAMPROPERTY = item.Url,
//			//        });
//			//    }
//			//}

//			//if (sysProgramList.Count > 0)
//			//{
//			//    SysProgramRepository.Insert(sysProgramList);
//			//}

//			////获取该用户的角色
//			//string userId = UserContext.GetCurrentUser().UserId;
//			//var sysRoleUser= SysRoleUserRepository.FirstOrDefault(t=>t.SYS_USERINFOID== userId);
//			//if (sysRoleUser == null)
//			//{
//			//    eventData.userNavigation.Items.Clear();
//			//    return;
//			//}

//			////获取该角色下的菜单
//			//var sysRoleAuthorize = SysRoleAuthorizeRespository.GetAllList(t => t.SYS_ROLEORUSERID == sysRoleUser.SYS_ROLEINFOID&&t.TYPE=="0");
//			//if (!sysRoleAuthorize.Any())
//			//{
//			//    eventData.userNavigation.Items.Clear();
//			//    return;
//			//}

//			//List<string> lists = sysRoleAuthorize.Select(x => x.SYS_POPEDOMPROGRAMID).ToList();
//			#endregion
//			var menus = MenuRepository.GetAllList().OrderBy(n => n.SORT).ToList();
//            #region new 

//            string[] menucodes = userMenus.Select(t => t.uId).ToArray();
//            var nowmenus = menus.Where(t => menucodes.Contains(t.CODE)).ToList();


//            List<UserNavigationItem> modelList = new List<UserNavigationItem>();
//            //
//            nowmenus = GetUpMenuList(nowmenus, menus, nowmenus);
//            nowmenus = nowmenus.OrderBy(n => n.SORT).ToList();
//            var parentMenus = nowmenus.Where(n => n.PID == null).ToList();
//            parentMenus.ForEach(item =>
//            {
//                var userNav = new UserNavigationItem(item.CODE, L(item.CODE))
//                {
//                    Icon = item.ICON
//                };
//                var addObj = GetItems(item, userNav, nowmenus, userMenus, sysPrograms);
//                modelList.Add(addObj);
//            });
//            eventData.userNavigation.Items.Clear();
//			foreach (var item in modelList)
//			{
//				eventData.userNavigation.Items.Add(item);
//			}
//			#endregion
//			#region old
//			//List<UserNavigationItem> menuItems = new List<UserNavigationItem>();
//			//foreach (var item in menus)
//			//{
//			//	UserNavigationItem userNavigation = new UserNavigationItem(item.CODE, L(item.CODE));
//			//	List<UserNavigationItem> userNavigationItems = new List<UserNavigationItem>();

//			//	var newSysPrograms = sysPrograms.Where(t => t.PROGRAMPARENT == item.Id).OrderBy(t => t.PROGRAMEXTEND);
//			//	foreach (var sysProgram in newSysPrograms)
//			//	{
//			//		var itemInfo = userMenus.FirstOrDefault(t => t.GetUniqueId() == sysProgram.PROGRAMCODE);
//			//		if (itemInfo != null)
//			//		{
//			//			userMenus.Remove(itemInfo);
//			//			userNavigationItems.Add(itemInfo);
//			//		}
//			//	}

//			//	if (userNavigationItems.Count > 0)
//			//	{
//			//		userNavigation.Items = userNavigationItems;
//			//		menuItems.Add(userNavigation);
//			//	}
//			//}

//			//eventData.userNavigation.Items.Clear();

//			//foreach (var item in menuItems)
//			//{
//			//	eventData.userNavigation.Items.Add(item);
//			//}
//			#endregion
//		}

//        public List<SYS_MENUS> GetUpMenuList(List<SYS_MENUS> nowmenus,List<SYS_MENUS> menus, List<SYS_MENUS> lastmenus)
//        {
//            var menuIds = nowmenus.Where(t => t.PID != null).GroupBy(t => new { t.PID })
//                       .Select(g => new
//                       {
//                           pId = g.Key.PID
//                       }
//                       ).ToList().Select(t=>t.pId).ToArray();
//            if (menuIds.Length > 0)
//            {
//                var nextmenus = menus.Where(t => menuIds.Contains(t.Id)).ToList();
//                foreach (var nextmenu in nextmenus)
//                {
//                    var exist = lastmenus.FirstOrDefault(t => t.Id == nextmenu.Id);
//                    if (exist != null)
//                    {
//                        continue;
//                    }
//                    lastmenus.Add(nextmenu);
//                }
//                lastmenus = GetUpMenuList(nextmenus, menus, lastmenus);
//            }
//            return lastmenus;
//        }

//        private UserNavigationItem GetItems(SYS_MENUS menu, UserNavigationItem userNav, List<SYS_MENUS> menus, IList<UserNavigationItem> userMenus, List<SYS_PROGRAM> sysPrograms, bool isRoot = true)
//		{
//			if (!isRoot)
//			{
//				var thisMenu = menus.Where(n => n.CODE == userNav.uId).FirstOrDefault();
//				userNav.Icon = thisMenu?.ICON;
//			}
//			var menuType = menu.TYPE;
//			//if (menuType == "0")
//			//{
//			//	var childSysPrograms = sysPrograms.Where(n => n.PROGRAMPARENT == menu.Id).ToList();
//			//	var mainMenu = new UserNavigationItem(item.CODE, L(item.CODE));
//			//}
//			//else
//			{
//				var childMenus = menus.Where(n => n.PID == menu.Id).ToList();
//				if (childMenus.Count > 0)
//				{
//					childMenus.ForEach(n =>
//					{
//						if (menuType == "0")
//						{
//							//var thisItem = userMenus.Where(i => i.uId == n.CODE).FirstOrDefault();
//							//var childSysPrograms = sysPrograms.Where(i => i.PROGRAMPARENT == menu.Id).ToList();
//							//var obj = new UserNavigationItem(n.CODE, L(n.CODE))
//							//{
//							//	Icon = n.ICON
//							//};
//							//var addObj = GetItems(n, obj, menus, userMenus, sysPrograms);
//							//userNav.Items.Add(addObj);
//							var obj = userMenus.FirstOrDefault(t => t.uId == n.CODE);
//							if (obj != null)
//							{
//								var thischildMenu = menus.Where(a => a.CODE == n.CODE).FirstOrDefault();
//								obj.Icon = thischildMenu?.ICON;
//								userNav.Items.Add(obj);
//							}
//							else
//							{
//								var objNew = new UserNavigationItem(n.CODE, L(n.CODE))
//								{
//									Icon = n.ICON
//								};
//								userNav.Items.Add(GetItems(n, objNew, menus, userMenus, sysPrograms));
//							}
//						}
//						else
//						{
//							var childSysPrograms = sysPrograms.Where(i => i.PROGRAMPARENT == menu.Id).ToList();
//							if (childSysPrograms.Count > 0)
//							{
//								childSysPrograms.ForEach(j =>
//								{
//									//var obj = new UserNavigationItem(j.PROGRAMNAME, L(j.PROGRAMCODE));

//									var obj = userMenus.FirstOrDefault(t => t.uId == j.PROGRAMCODE);
//									if (obj != null)
//									{
//										var thischildMenu = menus.Where(a => a.CODE == j.PROGRAMCODE).FirstOrDefault();
//										obj.Icon = thischildMenu?.ICON;
//										userNav.Items.Add(obj);
//									}
//									//var obj = userMenus.FirstOrDefault(t => t.GetUniqueId() == j.PROGRAMCODE);
//									//var thischildMenu = menus.Where(a => a.CODE == j.PROGRAMCODE).FirstOrDefault();
//									//item.Icon = thischildMenu?.ICON;
//									//item.Items.Add(obj);
//								});
//							}

//							//var thisItem = userMenus.Where(i => i.uId == n.CODE).FirstOrDefault();
//							//if (thisItem != null)
//							//{
//							//	//var obj = new UserNavigationItem(thisItem.Name, thisItem.DisplayName);
//							//	//item.Items.Add(thisItem);
//							//	GetItems(n, thisItem, menus, userMenus, sysPrograms, false);
//							//}
//						}
//					});
//				}
//				else
//				{
//					var childSysPrograms = sysPrograms.Where(i => i.PROGRAMPARENT == menu.Id).ToList();
//					if (childSysPrograms.Count > 0)
//					{
//						childSysPrograms.ForEach(n =>
//						{
//							//var obj = new UserNavigationItem(n.PROGRAMNAME, n.PROGRAMCODE);
//							var obj = userMenus.FirstOrDefault(t => t.uId == n.PROGRAMCODE);
//							if (obj != null)
//							{
//								var thischildMenu = menus.Where(a => a.CODE == n.PROGRAMCODE).FirstOrDefault();
//								obj.Icon = thischildMenu?.ICON;
//								userNav.Items.Add(obj);
//							}
//						});
//					}
//				}
//			}
//			//var itemsCount = item.Items.Count;
//			//var mainMenu = userMenus.Where(n => n.uId == code).FirstOrDefault();
//			//if (itemsCount > 0)
//			//{
//			//	item.Items.ToList().ForEach(n =>
//			//	{
//			//		var childMenu = userMenus.Where(i => i.uId == n.uId).FirstOrDefault();
//			//		if (childMenu != null)
//			//		{
//			//			GetItems(n, menus, userMenus);
//			//		}
//			//	});
//			//}
//			return userNav;
//		}
//	}
//}
