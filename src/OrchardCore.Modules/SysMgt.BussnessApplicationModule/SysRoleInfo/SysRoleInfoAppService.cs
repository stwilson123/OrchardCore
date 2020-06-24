using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Localization.Abtractions;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.SysProgram;
using SysMgt.BussnessDomainModule.SysRoleInfo;
using SysMgt.BussnessDomainModule.SysRoleUser;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.SysProgram;
using SysMgt.BussnessDTOModule.SysRoleInfo;
using SysMgt.BussnessDTOModule.SysUserInfo;


namespace SysMgt.BussnessApplicationModule
{
    public class SysRoleInfoAppService : AppService, ISysRoleInfoAppService
    {
        public SysRoleInfoDomain sysRoleInfoDomain { get; set; }
        public SysRoleInfoAppService(SysRoleInfoDomain sysRoleInfoDomain)
        {
            this.sysRoleInfoDomain = sysRoleInfoDomain;
        }
        public PageList<SysRoleInfoPageResult> GetPageList(SysRoleInfoSearchModel search)
        {
            return sysRoleInfoDomain.GetPageList(search);
        }
        
        public PageList<SysRoleInfoPageResult> GetUserAuList(SysRoleInfoSearchModel search)
        {
            return sysRoleInfoDomain.GetUserAuList(search);
        }
        
       public List<SysRoleInfoPageResult> GetUserNotRoleList(SysRoleInfoSearchModel search){
            return sysRoleInfoDomain.GetUserNotRoleList(search);
        }

        [LocalizedDescription("SysRoleInfoManager_Add")]
        public string Add([LocalizedDescription("add")]SysRoleInfo sysRoleInfo)
        {
            SysRoleInfoData sysRoleInfoData=new SysRoleInfoData();
            sysRoleInfoData.Name = sysRoleInfo.Name;
            sysRoleInfoData.Remark = sysRoleInfo.Remark;
           return sysRoleInfoDomain.Add(sysRoleInfoData);
        }
        [LocalizedDescription("SysRoleInfoManager_Delete")]
        public string Delete([LocalizedDescription("delete")]SysRoleInfo sysRoleInfo)
        {
            SysRoleInfoData sysRoleInfoData = new SysRoleInfoData();
            sysRoleInfoData.IDS = sysRoleInfo.IDS;
          return   sysRoleInfoDomain.Delete(sysRoleInfoData);
        }
        [LocalizedDescription("SysRoleInfoManager_Edit")]
        public string Edit([LocalizedDescription("edit")]SysRoleInfo sysRoleInfo)
        {
            SysRoleInfoData sysRoleInfoData = new SysRoleInfoData();
            sysRoleInfoData.ID = sysRoleInfo.ID;
            sysRoleInfoData.Name = sysRoleInfo.Name;
            sysRoleInfoData.Remark = sysRoleInfo.Remark;
           return sysRoleInfoDomain.Edit(sysRoleInfoData);
        }
        //[LocalizedDescription("分配菜单")]
        public void Allot(SysRoleInfo sysRoleInfo)
        {

            SysRoleInfoData sysRoleInfoData = new SysRoleInfoData();
            sysRoleInfoData.ID = sysRoleInfo.ID;

            List<SysProgramData> list=new List<SysProgramData>();
            foreach (var item in sysRoleInfo.SysProgramInfos)
            {
                list.Add(new SysProgramData(){ID = item.ID,URL = item.URL,Type = item.Type,PID=item.PID});
            }

            sysRoleInfoData.SysProgramDatas = list;
            sysRoleInfoDomain.Allot(sysRoleInfoData);
        }

        public SysRoleInfo GetOneById(SysRoleInfo sysRoleInfo)
        {
            SysRoleInfoData sysRoleInfoData=new SysRoleInfoData();
            sysRoleInfoData.ID = sysRoleInfo.ID;
            sysRoleInfoData = sysRoleInfoDomain.GetOneById(sysRoleInfoData);
            sysRoleInfo.Name = sysRoleInfoData.Name;
            sysRoleInfo.Remark= sysRoleInfoData.Remark;

            return sysRoleInfo;



        }

        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return sysRoleInfoDomain.GetComboxList(search);
        }

        public List<SysRoleInfoinfo> GetALLList(SearchModel search)
        {
            return sysRoleInfoDomain.GetALLList(search);
        }

        public List<SysPogramTree> GetAllSysProgram(SysRoleInfo sysRoleInfo)
        {
            SysRoleInfoData sysRoleInfoData=new SysRoleInfoData();
            sysRoleInfoData.ID = sysRoleInfo.ID;
            List<SysProgramTreeData> sysProgramTreeDatas = sysRoleInfoDomain.GetAllSysProgram(sysRoleInfoData);
            List<SysPogramTree> sysProgramInfos=new List<SysPogramTree>();
            foreach (var item in sysProgramTreeDatas)
            {
                sysProgramInfos.Add(new SysPogramTree()
                {
                    id = item.id,
                    pId = item.pId,
                    name = item.name,
                    @checked = item.@checked,
                    url = item.url,
                    type=item.type,
                    urlkey=item.urlkey
                });
            }

            return sysProgramInfos;
        }

        public ELsysPogramTreeCheckedNode GetAllELSysProgram(SysRoleInfo sysRoleInfo)
        {
            SysRoleInfoData sysRoleInfoData = new SysRoleInfoData();
            sysRoleInfoData.ID = sysRoleInfo.ID;
            var ELSysProgramTreeDatas = new ELsysPogramTreeCheckedNode();
            ELSysProgramTreeDatas = sysRoleInfoDomain.GetAllELSysProgram(sysRoleInfoData);
            return ELSysProgramTreeDatas;
        }

        public SysRoleAndUserInfo GetSysUserByRole(SysRoleInfo sysRoleInfo)
        {
            SysRoleInfoData sysRoleInfoData = new SysRoleInfoData();
            sysRoleInfoData.ID = sysRoleInfo.ID;
            return sysRoleInfoDomain.GetSysUserByRole(sysRoleInfoData);
        }
        [LocalizedDescription("SYS_BINGDINGUSER")]
        public string SaveRoleAndUser([LocalizedDescription("SYS_BINGDINGUSER")]SysRoleAndUserInfo sysRoleAndUserInfo)
        {
            SysRoleAndUserData sysRoleUserData = new SysRoleAndUserData();
            sysRoleUserData.RoleInfoID = sysRoleAndUserInfo.RoleInfoID;
            sysRoleUserData.Ids = sysRoleAndUserInfo.Ids;
            return sysRoleInfoDomain.SaveRoleAndUser(sysRoleUserData);
        }

        [LocalizedDescription("SYS_ROLECOPY")]
        public string SaveRoleMenu([LocalizedDescription("SYS_ROLECOPY")]SysRoleAndUserInfo sysRoleAndUserInfo)
        {
            SysRoleAndUserData sysRoleUserData = new SysRoleAndUserData();
            sysRoleUserData.RoleInfoID = sysRoleAndUserInfo.RoleInfoID;
            sysRoleUserData.Ids = sysRoleAndUserInfo.Ids;
            return sysRoleInfoDomain.SaveRoleMenu(sysRoleUserData);
        }

        public string DelAuList(SysRoleAndUserInfo sysRoleAndUserInfo)
        {
            SysRoleAndUserData sysRoleUserData = new SysRoleAndUserData();
            sysRoleUserData.Ids = sysRoleAndUserInfo.Ids;
          
            return sysRoleInfoDomain.DelAuList(sysRoleUserData);
        }

       
    }
}
