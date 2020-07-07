using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.Security;
using Microsoft.Extensions.Localization;
using BlocksCore.Navigation.Abstractions;
using SysMgt.BussnessRespositoryModule;

namespace SysMgt.BussnessDomainModule.SysProgram
{
    public class NavigationContructFilter : INavigationFilter
    {
        public ISysProgramRepository SysProgramRepository { get; set; }
        public IMenuRepository MenuRepository { get; set; }
        private IUserContext UserContext { get; set; }
        public ISysRoleUserRepository SysRoleUserRepository { get; set; }
        public ISysRoleAuthorizeRespository SysRoleAuthorizeRespository { get; set; }

        public IStringLocalizer L { get; set; }

        public NavigationContructFilter(ISysProgramRepository SysProgramRepository, IMenuRepository MenuRepository, ISysRoleUserRepository SysRoleUserRepository, ISysRoleAuthorizeRespository SysRoleAuthorizeRespository, IUserContext UserContext, IStringLocalizer<NavigationContructFilter> l)
        {
            this.SysProgramRepository = SysProgramRepository;
            this.MenuRepository = MenuRepository;
            this.UserContext = UserContext;
            this.SysRoleUserRepository = SysRoleUserRepository;
            this.SysRoleAuthorizeRespository = SysRoleAuthorizeRespository;
            L = l;
        }
        public IEnumerable<NavigationItem> OnFilterExecuted(IEnumerable<NavigationItem> navigationItems)
        {
            return navigationItems;
        }

        public IEnumerable<NavigationItem> OnFilterExecuting(IEnumerable<NavigationItem> navigationItems)
        {
            var userMenus = navigationItems.ToList();
            var sysPrograms = SysProgramRepository.GetAllList();
            var menus = MenuRepository.GetAllList().OrderBy(n => n.SORT).ToList();
            string[] menucodes = userMenus.Select(t => t.uId).ToArray();
            var nowmenus = menus.Where(t => menucodes.Contains(t.CODE)).ToList();


            List<NavigationItem> modelList = new List<NavigationItem>();
            nowmenus = GetUpMenuList(nowmenus, menus, nowmenus);
            nowmenus = nowmenus.OrderBy(n => n.SORT).ToList();
            var parentMenus = nowmenus.Where(n => n.PID == null).ToList();
            parentMenus.ForEach(item =>
            {
                var userNav = new NavigationItem(item.CODE)
                {
                    DisplayName= L[item.CODE],
                    Icon = item.ICON
                };
                var addObj = GetItems(item, userNav, nowmenus, userMenus, sysPrograms);
                modelList.Add(addObj);
            });
            //eventData.userNavigation.Items.Clear();
            //foreach (var item in modelList)
            //{
            //    eventData.userNavigation.Items.Add(item);
            //}
            return modelList;

        }


        public List<SYS_MENUS> GetUpMenuList(List<SYS_MENUS> nowmenus, List<SYS_MENUS> menus, List<SYS_MENUS> lastmenus)
        {
            var menuIds = nowmenus.Where(t => t.PID != null).GroupBy(t => new { t.PID })
                       .Select(g => new
                       {
                           pId = g.Key.PID
                       }
                       ).ToList().Select(t => t.pId).ToArray();
            if (menuIds.Length > 0)
            {
                var nextmenus = menus.Where(t => menuIds.Contains(t.Id)).ToList();
                foreach (var nextmenu in nextmenus)
                {
                    var exist = lastmenus.FirstOrDefault(t => t.Id == nextmenu.Id);
                    if (exist != null)
                    {
                        continue;
                    }
                    lastmenus.Add(nextmenu);
                }
                lastmenus = GetUpMenuList(nextmenus, menus, lastmenus);
            }
            return lastmenus;
        }

        private NavigationItem GetItems(SYS_MENUS menu, NavigationItem userNav, List<SYS_MENUS> menus, IList<NavigationItem> userMenus, List<SYS_PROGRAM> sysPrograms, bool isRoot = true)
        {
            if (!isRoot)
            {
                var thisMenu = menus.Where(n => n.CODE == userNav.uId).FirstOrDefault();
                userNav.Icon = thisMenu?.ICON;
            }
            var menuType = menu.TYPE;
            //if (menuType == "0")
            //{
            //	var childSysPrograms = sysPrograms.Where(n => n.PROGRAMPARENT == menu.Id).ToList();
            //	var mainMenu = new UserNavigationItem(item.CODE, L(item.CODE));
            //}
            //else
            {
                var childMenus = menus.Where(n => n.PID == menu.Id).ToList();
                if (childMenus.Count > 0)
                {
                    childMenus.ForEach(n =>
                    {
                        if (menuType == "0")
                        {
                            //var thisItem = userMenus.Where(i => i.uId == n.CODE).FirstOrDefault();
                            //var childSysPrograms = sysPrograms.Where(i => i.PROGRAMPARENT == menu.Id).ToList();
                            //var obj = new UserNavigationItem(n.CODE, L(n.CODE))
                            //{
                            //	Icon = n.ICON
                            //};
                            //var addObj = GetItems(n, obj, menus, userMenus, sysPrograms);
                            //userNav.Items.Add(addObj);
                            var obj = userMenus.FirstOrDefault(t => t.uId == n.CODE);
                            if (obj != null)
                            {
                                var thischildMenu = menus.Where(a => a.CODE == n.CODE).FirstOrDefault();
                                obj.Icon = thischildMenu?.ICON;
                                userNav.Items.Add(obj);
                            }
                            else
                            {
                                var objNew = new NavigationItem(n.CODE)
                                {
                                    Icon = n.ICON,
                                    DisplayName = L[n.CODE]
                                };
                                userNav.Items.Add(GetItems(n, objNew, menus, userMenus, sysPrograms));
                            }
                        }
                        else
                        {
                            var childSysPrograms = sysPrograms.Where(i => i.PROGRAMPARENT == menu.Id).ToList();
                            if (childSysPrograms.Count > 0)
                            {
                                childSysPrograms.ForEach(j =>
                                {
                                    //var obj = new UserNavigationItem(j.PROGRAMNAME, L(j.PROGRAMCODE));

                                    var obj = userMenus.FirstOrDefault(t => t.uId == j.PROGRAMCODE);
                                    if (obj != null)
                                    {
                                        var thischildMenu = menus.Where(a => a.CODE == j.PROGRAMCODE).FirstOrDefault();
                                        obj.Icon = thischildMenu?.ICON;
                                        userNav.Items.Add(obj);
                                    }
                                    //var obj = userMenus.FirstOrDefault(t => t.GetUniqueId() == j.PROGRAMCODE);
                                    //var thischildMenu = menus.Where(a => a.CODE == j.PROGRAMCODE).FirstOrDefault();
                                    //item.Icon = thischildMenu?.ICON;
                                    //item.Items.Add(obj);
                                });
                            }

                            //var thisItem = userMenus.Where(i => i.uId == n.CODE).FirstOrDefault();
                            //if (thisItem != null)
                            //{
                            //	//var obj = new UserNavigationItem(thisItem.Name, thisItem.DisplayName);
                            //	//item.Items.Add(thisItem);
                            //	GetItems(n, thisItem, menus, userMenus, sysPrograms, false);
                            //}
                        }
                    });
                }
                else
                {
                    var childSysPrograms = sysPrograms.Where(i => i.PROGRAMPARENT == menu.Id).ToList();
                    if (childSysPrograms.Count > 0)
                    {
                        childSysPrograms.ForEach(n =>
                        {
                            //var obj = new UserNavigationItem(n.PROGRAMNAME, n.PROGRAMCODE);
                            var obj = userMenus.FirstOrDefault(t => t.uId == n.PROGRAMCODE);
                            if (obj != null)
                            {
                                var thischildMenu = menus.Where(a => a.CODE == n.PROGRAMCODE).FirstOrDefault();
                                obj.Icon = thischildMenu?.ICON;
                                userNav.Items.Add(obj);
                            }
                        });
                    }
                }
            }
            //var itemsCount = item.Items.Count;
            //var mainMenu = userMenus.Where(n => n.uId == code).FirstOrDefault();
            //if (itemsCount > 0)
            //{
            //	item.Items.ToList().ForEach(n =>
            //	{
            //		var childMenu = userMenus.Where(i => i.uId == n.uId).FirstOrDefault();
            //		if (childMenu != null)
            //		{
            //			GetItems(n, menus, userMenus);
            //		}
            //	});
            //}
            return userNav;
        }
    }
}
