using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Domain.Abstractions.Domain;
using Blocks.BussnessEntityModule;

using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.Localization;
using SysMgt.BussnessDomainModule.SysProgram;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.SysRoleInfo;
using SysMgt.BussnessRespositoryModule;
using BlocksCore.Abstractions.Security;
using SysMgt.BussnessDTOModule.SysUserInfo;
using SysMgt.BussnessDomainModule.SysRoleUser;
using SysMgt.BussnessDTOModule.SysProgram;
using BlocksCore.Navigation.Abstractions;
using BlocksCore.Event.Abstractions;
using BlocksCore.Infrastructure.Abstractions.Security.Permissions.Event;
using BlocksCore.Abstractions;
using BlocksCore.Infrastructure.Abstractions.Security.Permissions;

namespace SysMgt.BussnessDomainModule.SysRoleInfo
{
    public class SysRoleInfoDomain : IDomainService
    {
        public ISysRoleInfoRepository SysRoleInfoRepository { get; set; }
        public ISysProgramRepository SysProgramRepository { get; set; }

        public IMenuRepository MenuRepository { get; set; }
        public ISysRoleUserRepository SysRoleUserRepository { get; set; }
        public ISysRoleAuthorizeRespository SysRoleAuthorizeRespository { get; set; }
        public IUserNavigationManager NavigationManager { get; set; }


        public IEventBus EventBus { get; set; }

        private IUserContext userContext;
        public IStringLocalizer L { get; set; }

        public List<SysProgramTreeData> listpProgramTreeDatas;

        public SysRoleInfoDomain(ISysRoleInfoRepository SysRoleInfoRepository, ISysProgramRepository SysProgramRepository, ISysRoleUserRepository SysRoleUserRepository, IMenuRepository MenuRepository, ISysRoleAuthorizeRespository SysRoleAuthorizeRespository, IUserNavigationManager navigationManager, IUserContext userContext)
        {
            this.SysRoleInfoRepository = SysRoleInfoRepository;
            this.SysProgramRepository = SysProgramRepository;
            this.SysRoleUserRepository = SysRoleUserRepository;
            this.MenuRepository = MenuRepository;
            this.SysRoleAuthorizeRespository = SysRoleAuthorizeRespository;
            this.NavigationManager = navigationManager;
            this.userContext = userContext;
        }

        public string Add(SysRoleInfoData sysRoleInfoData)
        {
            var sysRoleInfo = SysRoleInfoRepository.FirstOrDefault(t => t.CNAME == sysRoleInfoData.Name);
            if (sysRoleInfo != null)
            {
                throw new BlocksBussnessException("101", L["名称重复"], null);
            }
            if (string.IsNullOrEmpty(sysRoleInfoData.Name.Trim()))
            {
                throw new BlocksBussnessException("101", L["名称不能为空!"], null);
            }
            SYS_ROLEINFO sysRoleinfo = new SYS_ROLEINFO();
            sysRoleinfo.Id = Guid.NewGuid().ToString();
            sysRoleinfo.CNAME = sysRoleInfoData.Name;
            sysRoleinfo.MEMO = sysRoleInfoData.Remark;
            string returnId = SysRoleInfoRepository.InsertAndGetId(sysRoleinfo);
            if (string.IsNullOrEmpty(returnId))
            {
                return "保存失败";
            }
            else
            {
                return "保存成功";
            }
        }

        public List<SysRoleInfoinfo> GetALLList(SearchModel search)
        {
            List<SysRoleInfoinfo> sysRoleInfoinfos = new List<SysRoleInfoinfo>();
            var sysRoleInfos = SysRoleInfoRepository.GetAllList();
            foreach (var sysRoleInfo in sysRoleInfos)
            {
                SysRoleInfoinfo sysRoleInfoinfo = new SysRoleInfoinfo();
                sysRoleInfoinfo.ID = sysRoleInfo.Id;
                sysRoleInfoinfo.Name = sysRoleInfo.CNAME;
                sysRoleInfoinfos.Add(sysRoleInfoinfo);
            }
            return sysRoleInfoinfos;
        }

        public string Edit(SysRoleInfoData sysRoleInfoData)
        {
            var sysRoleInfo = SysRoleInfoRepository.FirstOrDefault(t => t.CNAME == sysRoleInfoData.Name && t.Id != sysRoleInfoData.ID);
            if (sysRoleInfo != null)
            {
                throw new BlocksBussnessException("101", L["名称重复"], null);
            }
            if (sysRoleInfoData.Name == "")
            {
                throw new BlocksBussnessException("101", L["名称不能为空!"], null);
            }
            int successCount = SysRoleInfoRepository.Update(t => t.Id == sysRoleInfoData.ID, t => new SYS_ROLEINFO()
            {
                CNAME = sysRoleInfoData.Name,
                MEMO = sysRoleInfoData.Remark
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

        public void Allot(SysRoleInfoData sysRoleInfoData)
        {
            List<SYS_ROLEAUTHORIZE> sysRoleauthorizes = new List<SYS_ROLEAUTHORIZE>();

            foreach (var item in sysRoleInfoData.SysProgramDatas)
            {
                if (item.Type == "0" || item.Type == null)
                {
                    sysRoleauthorizes.Add(new SYS_ROLEAUTHORIZE()
                    {
                        Id = Guid.NewGuid().ToString(),
                        SYS_POPEDOMPROGRAMID = item.ID,
                        SYS_ROLEORUSERID = sysRoleInfoData.ID,
                        TYPE = "0"
                    });
                }
                else
                {
                    sysRoleauthorizes.Add(new SYS_ROLEAUTHORIZE()
                    {
                        Id = Guid.NewGuid().ToString(),
                        SYS_POPEDOMPROGRAMID = item.PID,
                        SYS_ROLEORUSERID = sysRoleInfoData.ID,
                        RESOURCE_KEY = item.URL,
                        TYPE = "1"
                    });
                }


            }
            //先删除该角色的所有权限
            SysRoleAuthorizeRespository.Delete(t => t.SYS_ROLEORUSERID == sysRoleInfoData.ID);
            SysRoleAuthorizeRespository.Insert(sysRoleauthorizes);
            EventBus.Trigger(new PermissionChangeEventData(sysRoleInfoData.ID));


        }


        public string Delete(SysRoleInfoData sysRoleInfoData)
        {
            for (var i = 0; i < sysRoleInfoData.IDS.Count; i++)
            {
                var alluser = SysRoleUserRepository.GetAllList(t => t.SYS_ROLEINFOID == sysRoleInfoData.IDS[i]);
                string name = SysRoleInfoRepository.FirstOrDefault(t => t.Id == sysRoleInfoData.IDS[i]).CNAME;
                if (alluser.Count != 0)
                {
                    throw new BlocksBussnessException("101", L[name + "下面有绑定用户，无法删除"], null);
                }
                var successCount = SysRoleInfoRepository.Delete(t => t.Id == sysRoleInfoData.IDS[i]);
                if (successCount <= 0)
                {
                    throw new BlocksBussnessException("101", L["Delete Failed"], null);
                }
            }
            return "删除成功";

        }

        public SysRoleInfoData GetOneById(SysRoleInfoData sysRoleInfoData)
        {
            var sysRoleInfo = SysRoleInfoRepository.FirstOrDefault(t => t.Id == sysRoleInfoData.ID);
            if (sysRoleInfo == null)
            {
                throw new BlocksBussnessException("101", L["Not querying data"], null);
            }

            sysRoleInfoData.Name = sysRoleInfo.CNAME;
            sysRoleInfoData.Remark = sysRoleInfo.MEMO;
            return sysRoleInfoData;
        }

        public PageList<SysRoleInfoPageResult> GetPageList(SysRoleInfoSearchModel search)
        {
            return SysRoleInfoRepository.GetPageList(search);
        }

        public List<SysRoleInfoPageResult> GetUserNotRoleList(SysRoleInfoSearchModel search)
        {
            var roleinfo = SysRoleInfoRepository.GetAllList().Select(i=> new SysRoleInfoPageResult{
                 ID = i.Id,
                  Name = i.CNAME,
                   Remark = i.MEMO
            }).ToList();

            var userRole = SysRoleUserRepository.GetAllList(i => i.SYS_USERINFOID == search.UserId).Select(i =>i.SYS_ROLEINFOID);


            roleinfo = roleinfo.Where(i => !userRole.Contains(i.ID)).ToList();

            return roleinfo;

        }



        public virtual PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return SysRoleInfoRepository.GetComboxList(search);
        }

        public ELsysPogramTreeCheckedNode GetAllELSysProgram(SysRoleInfoData sysRoleInfoData)
        {
            var sysPrograms = SysProgramRepository.GetAllList(t => t.PROGRAMPARENT != null || t.PROGRAMPARENT == "");
            var sysMenu = MenuRepository.GetAllList();
            var sysRoleAuths = SysRoleAuthorizeRespository.GetAllList(t => t.SYS_ROLEORUSERID == sysRoleInfoData.ID);
            var sysUserSysProgram = sysRoleAuths.Where(t => t.TYPE == "0").ToList();
            var sysUserAction = sysRoleAuths.Where(t => t.TYPE == "1").ToList();
            //List<SysProgramTreeData> listpProgramTreeDatas = new List<SysProgramTreeData>();
            listpProgramTreeDatas = new List<SysProgramTreeData>();
            //var mainMenu = UserNavigationManager.GetMenuAsync("MainMenu", UserContext.GetCurrentUser());
            //var mainMenuList = mainMenu.Result.Items;
            // var mainMenu = NavigationManager.MainMenu;
            var mainMenu = NavigationManager.GetFilterMenuAsync(Platform.Main.ToString()).Result;

            var mainMenuList = mainMenu;


            foreach (var menuItem in sysMenu)
            {
                var programsmenu = sysPrograms.Where(t => t.PROGRAMPARENT == menuItem.Id).OrderBy(t => t.PROGRAMEXTEND);

                var programs = sysMenu.Where(t => t.PID == menuItem.Id).OrderBy(t => t.SORT);

                if (menuItem.TYPE == "0")
                {
                    bool isCheck = false;
                    var menus = sysMenu.Where(t => t.PID == menuItem.Id);
                    foreach (var i in menus)
                    {
                        isCheck = sysUserSysProgram.Exists(t => t.SYS_POPEDOMPROGRAMID == i.Id);
                        break;
                    }
                    SysProgramTreeData sysProgramTreeData = new SysProgramTreeData()
                    {
                        id = menuItem.Id,
                        pId = menuItem.PID,
                        name = L[menuItem.CODE],
                        @checked = isCheck,
                        url = "",
                        type = "0"
                    };
                    listpProgramTreeDatas.Add(sysProgramTreeData);
                }
                else if (menuItem.TYPE == "1")
                {
                    var syspitem = sysPrograms.FirstOrDefault(t => t.Id == menuItem.Id);
                    if (syspitem != null)
                    {
                        foreach (var menu in mainMenuList)
                        {
                            if (menu.uId == syspitem.PROGRAMCODE)
                            {
                                menuItem.NAME = menu.DisplayName;
                                break;
                            }
                        }
                        bool isCheck = sysUserSysProgram.Exists(t => t.SYS_POPEDOMPROGRAMID == syspitem.Id);
                        SysProgramTreeData sysProgramTreeData = new SysProgramTreeData()
                        {
                            id = menuItem.Id,
                            pId = menuItem.PID,
                            name = menuItem.NAME,
                            @checked = isCheck,
                            url = "",// syspitem.PROGRAMPROPERTY,
                            urlkey = syspitem.PROGRAMPROPERTY,
                            type = "0"
                        };
                        listpProgramTreeDatas.Add(sysProgramTreeData);

                        GetBtnInfos(syspitem, sysUserAction, sysPrograms);
                    }
                }

            }
            var ELSysPogramTreeData = new List<ELSysPogramTree>();

            foreach (var item in listpProgramTreeDatas.Where(i => i.pId == null))
            {
                var newData = new ELSysPogramTree()
                {
                    id = item.id,
                    name = item.name,
                    @checked = item.@checked,
                    URL = item.url,
                    PID = item.pId,
                    type = item.type,
                    urlkey = item.urlkey
                };
                newData.children = ELSysPogramTreeChildData(listpProgramTreeDatas, newData);
                ELSysPogramTreeData.Add(newData);
            }

            var TreeCheckedNodeIDs = listpProgramTreeDatas.Where(i => i.@checked == true).Select(i => i.id).ToList();

            return new ELsysPogramTreeCheckedNode
            {
                ELSysPogramTreeDatas = ELSysPogramTreeData,
                TreeCheckedNodeIDs = TreeCheckedNodeIDs
            };
        }

        public List<ELSysPogramTree> ELSysPogramTreeChildData(List<SysProgramTreeData> listpProgramTreeDatas,
            ELSysPogramTree sysPogramTree)
        {
            var ELSysPogramTreeData = new List<ELSysPogramTree>();
            foreach (var item in listpProgramTreeDatas.Where(i => i.pId == sysPogramTree.id))
            {
                var newData = new ELSysPogramTree()
                {
                    id = item.id,
                    name = item.name,
                    @checked = item.@checked,
                    URL = item.url,
                    PID = item.pId,
                    type = item.type,
                    urlkey = item.urlkey
                };
                newData.children = ELSysPogramTreeChildData(listpProgramTreeDatas, newData);
                ELSysPogramTreeData.Add(newData);
            }

            return ELSysPogramTreeData;
        }

        public List<SysProgramTreeData> GetAllSysProgram(SysRoleInfoData sysRoleInfoData)
        {

            var sysPrograms = SysProgramRepository.GetAllList(t => t.PROGRAMPARENT != null || t.PROGRAMPARENT == "");
            var sysMenu = MenuRepository.GetAllList();
            var sysRoleAuths = SysRoleAuthorizeRespository.GetAllList(t => t.SYS_ROLEORUSERID == sysRoleInfoData.ID);
            var sysUserSysProgram = sysRoleAuths.Where(t =>t.TYPE == "0").ToList();
            var sysUserAction = sysRoleAuths.Where(t =>t.TYPE == "1").ToList();
            //List<SysProgramTreeData> listpProgramTreeDatas = new List<SysProgramTreeData>();
            listpProgramTreeDatas = new List<SysProgramTreeData>();
            //var mainMenu = UserNavigationManager.GetMenuAsync("MainMenu", UserContext.GetCurrentUser());
            //var mainMenuList = mainMenu.Result.Items;
            var mainMenu = NavigationManager.GetFilterMenuAsync(Platform.Main.ToString()).Result;
            var mainMenuList = mainMenu;


            foreach (var menuItem in sysMenu)
            {
                var programsmenu = sysPrograms.Where(t => t.PROGRAMPARENT == menuItem.Id).OrderBy(t => t.PROGRAMEXTEND);

                var programs = sysMenu.Where(t => t.PID == menuItem.Id).OrderBy(t => t.SORT);

                #region
                //if (programs.Any())
                //{
                //    SysProgramTreeData sysProgramTreeData = new SysProgramTreeData()
                //    {
                //        id = menuItem.Id,
                //        pId = menuItem.PID,
                //        name = menuItem.NAME.AutoMapTo<string>(),
                //        @checked = false,
                //        url = "",
                //        //type="0",
                //    };

                //    foreach (var item in programsmenu)
                //    {

                //        foreach (var menu in mainMenuList)
                //        {
                //            if (menu.GetUniqueId() == item.PROGRAMCODE)
                //            {
                //                item.PROGRAMNAME = menu.DisplayName;
                //                break;
                //            }
                //        }

                //        bool isCheck = sysUserSysProgram.Exists(t => t.SYS_POPEDOMPROGRAMID == item.Id);
                //        listpProgramTreeDatas.Add(new SysProgramTreeData()
                //        {
                //            id = item.Id,
                //            pId = item.PROGRAMPARENT,
                //            name = item.PROGRAMNAME,
                //            @checked = isCheck,
                //            url = item.PROGRAMPROPERTY,
                //            type = "0"
                //        });

                //        if (isCheck && !sysProgramTreeData.@checked)
                //        {
                //            sysProgramTreeData.@checked = true;
                //        }

                //        var nav = NavigationManager.MainMenu.Items.Where(t => t.GetUniqueId() == item.PROGRAMCODE).FirstOrDefault();

                //        if (nav != null && nav.HasPermissions != null)
                //        {

                //            Permission[] permissions = nav.HasPermissions;

                //            foreach (var actionItem in permissions)
                //            {

                //                var isExist = sysUserAction.Exists(t => t.SYS_POPEDOMPROGRAMID == item.Id && t.RESOURCE_KEY == actionItem.ResourceKey);              
                //                if (isExist)
                //                {
                //                    listpProgramTreeDatas.Add(new SysProgramTreeData()
                //                    {
                //                        id = Guid.NewGuid().ToString(),
                //                        pId = item.Id,
                //                        name = actionItem.DisplayName.AutoMapTo<string>(),
                //                        @checked = isExist,
                //                        url = actionItem.ResourceKey,
                //                        type = "1"
                //                    });

                //                }
                //                else
                //                {
                //                    listpProgramTreeDatas.Add(new SysProgramTreeData()
                //                    {
                //                        id = Guid.NewGuid().ToString(),
                //                        pId = item.Id,
                //                        name = actionItem.DisplayName.AutoMapTo<string>(),
                //                        @checked = isExist,
                //                        url = actionItem.ResourceKey,
                //                        type = "1"
                //                    });
                //                    //foreach (var b in aPId)
                //                    //{
                //                    //    listpProgramTreeDatas.Add(new SysProgramTreeData()
                //                    //    {
                //                    //        id = Guid.NewGuid().ToString(),
                //                    //        pId = b.Id,
                //                    //        name = b.PROGRAMNAME.AutoMapTo<string>(),
                //                    //        @checked = isExist,
                //                    //        url = actionItem.ResourceKey,
                //                    //        type = "1"
                //                    //    });
                //                    //}


                //                }
                //            }

                //        }



                //    }

                //    listpProgramTreeDatas.Add(sysProgramTreeData);
                //}
                #endregion

                if (menuItem.TYPE == "0")
                {
                    bool isCheck = false;
                    var menus = sysMenu.Where(t => t.PID == menuItem.Id);
                    foreach (var i in menus)
                    {
                        isCheck = sysUserSysProgram.Exists(t => t.SYS_POPEDOMPROGRAMID == i.Id);
                        break;
                    }
                    SysProgramTreeData sysProgramTreeData = new SysProgramTreeData()
                    {
                        id = menuItem.Id,
                        pId = menuItem.PID,
                        name = L[menuItem.CODE],
                        @checked = isCheck,
                        url = "",
                        type = "0"
                    };
                    listpProgramTreeDatas.Add(sysProgramTreeData);
                }
                else if (menuItem.TYPE == "1")
                {
                    var syspitem = sysPrograms.FirstOrDefault(t => t.Id == menuItem.Id);
                    if (syspitem != null)
                    {
                        foreach (var menu in mainMenuList)
                        {
                            if (menu.uId == syspitem.PROGRAMCODE)
                            {
                                menuItem.NAME = menu.DisplayName;
                                break;
                            }
                        }
                        bool isCheck = sysUserSysProgram.Exists(t => t.SYS_POPEDOMPROGRAMID == syspitem.Id);
                        SysProgramTreeData sysProgramTreeData = new SysProgramTreeData()
                        {
                            id = menuItem.Id,
                            pId = menuItem.PID,
                            name = menuItem.NAME,
                            @checked = isCheck,
                            url = "",// syspitem.PROGRAMPROPERTY,
                            urlkey = syspitem.PROGRAMPROPERTY,
                            type = "0"
                        };
                        listpProgramTreeDatas.Add(sysProgramTreeData);

                        GetBtnInfos(syspitem, sysUserAction, sysPrograms);
                    }
                }
                else { }


            }

            return listpProgramTreeDatas;

        }

        public void GetBtnInfos(SYS_PROGRAM item,List<SYS_ROLEAUTHORIZE> sysUserAction,List<SYS_PROGRAM> sysPrograms)
        {
            var menuList = NavigationManager.GetFilterMenuAsync(Platform.Main.ToString()).Result;
            var nav = menuList.Where(t => t.uId == item.PROGRAMCODE).FirstOrDefault();
            
            if (nav != null && nav.Permissions != null)
            {
                Permission[] permissions = nav.Permissions;

                foreach (var actionItem in permissions)
                {
                    var isExist = sysUserAction.Exists(t => t.SYS_POPEDOMPROGRAMID == item.Id && t.RESOURCE_KEY == actionItem.ResourceKey);
                    var id = Guid.NewGuid().ToString();

                    listpProgramTreeDatas.Add(new SysProgramTreeData()
                    {
                        id = id,
                        pId = item.Id,
                        name = actionItem.DisplayName,
                        @checked = isExist,
                        url = "",
                        urlkey = actionItem.ResourceKey,
                        //url = actionItem.ResourceKey,
                        type = "1"
                    });
                }

                //var syspitemchilds = SysProgramRepository.GetAllList(t => t.PROGRAMPARENT == item.Id);

                //foreach (var syspitemchild in syspitemchilds)
                //{
                //    A(syspitemchild);
                //}
            }

        }
        public virtual SysRoleAndUserInfo GetSysUserByRole(SysRoleInfoData sysRoleInfoData)
        {
            if (sysRoleInfoData == null || string.IsNullOrEmpty(sysRoleInfoData.ID))
            {
                throw new BlocksBussnessException("101", L["未传入角色信息!"], null);
            }
            SysRoleAndUserInfo rtnData = new SysRoleAndUserInfo();
            var list = SysRoleUserRepository.GetAllList(x => x.SYS_ROLEINFOID == sysRoleInfoData.ID);
            List<string> ids = new List<string>();
            //List<SYS_ROLEUSER> s = new List<SYS_ROLEUSER>();  
            foreach (var item in list)
            {
                ids.Add(item.SYS_USERINFOID);

            }
            rtnData.Ids = ids;
            return rtnData;

        }

        public string SaveRoleAndUser(SysRoleAndUserData sysRoleAndUserData)
        {

            //var userInfoIds = SysRoleUserRepository.GetAllList(t => sysRoleAndUserData.Ids.Contains(t.SYS_USERINFOID)).Select(i => i.SYS_USERINFOID);

            SysRoleUserRepository.Delete(t => t.SYS_ROLEINFOID == sysRoleAndUserData.RoleInfoID );
            
            if (sysRoleAndUserData != null)
            {
                List<SYS_ROLEUSER> roleUsers = new List<SYS_ROLEUSER>();

                foreach (var item in sysRoleAndUserData.Ids)
                {
                    SYS_ROLEUSER roleUser = new SYS_ROLEUSER();
                    roleUser.Id = Guid.NewGuid().ToString();
                    roleUser.SYS_ROLEINFOID = sysRoleAndUserData.RoleInfoID;
                    roleUser.SYS_USERINFOID = item;
                    roleUsers.Add(roleUser);
                }

                SysRoleUserRepository.Insert(roleUsers);
            }
            return "保存成功";
        }
        public string SaveRoleMenu(SysRoleAndUserData sysRoleAndUserData)
        {
            //通过sysRoleAndUserData.RoleInfoID来获取复制的权限菜单
            var roleMenu = SysRoleAuthorizeRespository.GetAllList(t => t.SYS_ROLEORUSERID == sysRoleAndUserData.RoleInfoID);
            //删除要绑定角色的权限
            SysRoleAuthorizeRespository.Delete(t => sysRoleAndUserData.Ids.Contains(t.SYS_ROLEORUSERID));


            if (sysRoleAndUserData != null)
            {
                List<SYS_ROLEAUTHORIZE> roleAuthorize = new List<SYS_ROLEAUTHORIZE>();
                foreach (var item in sysRoleAndUserData.Ids)
                {
                    foreach (var roleMenus in roleMenu)
                    {

                        SYS_ROLEAUTHORIZE roleUser = new SYS_ROLEAUTHORIZE();
                        roleUser.Id = Guid.NewGuid().ToString();
                        roleUser.SYS_POPEDOMPROGRAMID = roleMenus.SYS_POPEDOMPROGRAMID;
                        // roleUser.SYS_PROGRAMOPERATION_ID = roleMenus.SYS_PROGRAMOPERATION_ID;
                        roleUser.TYPE = roleMenus.TYPE;
                        roleUser.RESOURCE_KEY = roleMenus.RESOURCE_KEY;
                        roleUser.SYS_ROLEORUSERID = item;
                        roleAuthorize.Add(roleUser);
                    }
                }
                SysRoleAuthorizeRespository.Insert(roleAuthorize);
                return "保存成功";
            }
            else
            {
                return "保存失败!";
            }

        }

        public PageList<SysRoleInfoPageResult> GetUserAuList(SysRoleInfoSearchModel search)
        {
            return SysRoleInfoRepository.GetUserAuList(search);
        }
        public virtual string DelAuList(SysRoleAndUserData sysRoleAndUserData)
        {
            for (var i = 0; i < sysRoleAndUserData.Ids.Count; i++)
            {
                var id = sysRoleAndUserData.Ids[i];
                var exsitData = SysRoleUserRepository.FirstOrDefault(t => t.SYS_ROLEINFOID == id);
                if (exsitData == null)
                {
                    throw new BlocksBussnessException("101", L["未查到对象"], null);
                }

                SysRoleUserRepository.Delete(t => t.SYS_ROLEINFOID == id);

            }
            return "删除成功！";
        }
    }
}
