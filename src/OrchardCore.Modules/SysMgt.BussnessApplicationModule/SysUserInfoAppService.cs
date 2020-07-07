using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using Microsoft.Extensions.Localization;
using SysMgt.BussnessDomainModule;
using SysMgt.BussnessDomainModule.SysRoleUser;
using SysMgt.BussnessDomainModule.SysUserInfo;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.SysRoleInfo;
using SysMgt.BussnessDTOModule.SysUserInfo;
using BlocksCore.Localization.Abtractions;

namespace SysMgt.BussnessApplicationModule
{
    public class SysUserInfoAppService : AppService, ISysUserInfoAppService
    {
        private SysUserInfoDomain sysUserInfoDomain { get; set; }
        public SysUserInfoAppService(SysUserInfoDomain sysUserInfoDomain)
        {
            this.sysUserInfoDomain = sysUserInfoDomain;
        }
        public PageList<SysUserInfoPageResult> GetPageList(SysUserInfoSearchModel search)
        {
             return sysUserInfoDomain.GetPageList(search);
        }

        public  List<SysUserInfoPageResult> GetRoleUserList(SysUserInfoSearchModel search)
        {
            return sysUserInfoDomain.GetRoleUserList(search);
        }
        public PageList<SysUserInfoPageResult> GetRoleAuList(SysUserInfoSearchModel search)
        {
            return sysUserInfoDomain.GetRoleAuList(search);
        }
        [LocalizedDescription("SYSUSERINFO_ADD")]
        public string Add([LocalizedDescription("add")]SysUserInfo sysUserInfo)
        {
            SysUserInfoData sysUserInfoEventData=new SysUserInfoData();
            sysUserInfoEventData.UserCode = sysUserInfo.UserCode;
            sysUserInfoEventData.CName = sysUserInfo.CName;
            sysUserInfoEventData.Password = sysUserInfo.Password;
            sysUserInfoEventData.State = 0;
            sysUserInfoEventData.Memo = sysUserInfo.Memo;
            return sysUserInfoDomain.Add(sysUserInfoEventData);
        }

        public  SysUserInfo GetOneById(SysUserInfo sysUserInfo)
        {
            SysUserInfoData sysUserInfoData = new SysUserInfoData();
            sysUserInfoData.ID = sysUserInfo.ID;

            sysUserInfoData = sysUserInfoDomain.GetOneById(sysUserInfoData);
            sysUserInfo.UserCode = sysUserInfoData.UserCode;
            sysUserInfo.CName = sysUserInfoData.CName;
            sysUserInfo.State = sysUserInfoData.State;
            sysUserInfo.Memo = sysUserInfoData.Memo;
            return sysUserInfo;

        }
        [LocalizedDescription("SYSUSERINFO_EDIT")]
        public string Update([LocalizedDescription("edit")]SysUserInfo sysUserInfo)
        {
            SysUserInfoData sysUserInfoEventData = new SysUserInfoData();
            sysUserInfoEventData.ID = sysUserInfo.ID;
            sysUserInfoEventData.UserCode = sysUserInfo.UserCode;
            sysUserInfoEventData.CName = sysUserInfo.CName;
           // sysUserInfoEventData.Password = sysUserInfo.Password;
          //  sysUserInfoEventData.State = sysUserInfo.State;
            sysUserInfoEventData.Memo = sysUserInfo.Memo;
            return sysUserInfoDomain.Update(sysUserInfoEventData);
        }


        [LocalizedDescription("PasswordModification")]
        public string PasswordModification([LocalizedDescription("PasswordModification")]SysUserPwdModificationInfo sysUserInfo)
        {
            return sysUserInfoDomain.PasswordModification(sysUserInfo);
        }

        [LocalizedDescription("validatorPassword")]
        public string validatorPassword([LocalizedDescription("validatorPassword")]SysUserPwdModificationInfo sysUserInfo)
        {
            return sysUserInfoDomain.validatorPassword(sysUserInfo);
        }
        //[LocalizedDescription("Allot")]
        public string Allot(AllotInfo allotInfo)
        {
            AllotData allotData=new AllotData();
            allotData.RoleID = allotInfo.RoleID;
            allotData.IDs = allotInfo.IDs;
           return sysUserInfoDomain.Allot(allotData);

        }
        [LocalizedDescription("DISABLE")]
        //停用
        public string Disable([LocalizedDescription("DISABLE")]SysUserInfo SysUserInfo)
        {
            SysUserInfoData SysUserInfoData = new SysUserInfoData();
            SysUserInfoData.ID = SysUserInfo.ID;
            return sysUserInfoDomain.Disable(SysUserInfoData);
        }
        [LocalizedDescription("ENABLE")]
        //启用
        public string Enable([LocalizedDescription("ENABLE")]SysUserInfo SysUserInfo)
        {
            SysUserInfoData SysUserInfoData = new SysUserInfoData();
            SysUserInfoData.ID = SysUserInfo.ID;
            return sysUserInfoDomain.Enable(SysUserInfoData);
        }
        [LocalizedDescription("PASSWORDRESET")]
        //启用
        public string PasswordReset([LocalizedDescription("PASSWORDRESET")]SysUserInfo SysUserInfo)
        {
            SysUserInfoData SysUserInfoData = new SysUserInfoData();
            SysUserInfoData.ID = SysUserInfo.ID;
            return sysUserInfoDomain.PasswordReset(SysUserInfoData);
        }

        

        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return sysUserInfoDomain.GetComboxList(search);
        }

        public string Login(SysLoginInfo sysLoginInfo)
        {
            SysLoginData sysLoginData = new SysLoginData();
            sysLoginData.Account = sysLoginInfo.Account;
            sysLoginData.Pwd = sysLoginInfo.Pwd;
            return sysUserInfoDomain.Login(sysLoginData);
        }

        public SysRoleUserInfo GetSysRoleByUser(SysUserInfo sysUserInfo) {
            SysUserInfoData sysUserInfoData = new SysUserInfoData();
            sysUserInfoData.ID = sysUserInfo.ID;
            return sysUserInfoDomain.GetSysRoleByUser(sysUserInfoData);
        }
        [LocalizedDescription("SYSUSERINFO_ALLOT")]
        public string SaveRoleUser([LocalizedDescription("SYSUSERINFO_ALLOT")]SysRoleUserInfo sysRoleUserInfo)
        {
            SysRoleUserData sysRoleUserData = new SysRoleUserData();
            sysRoleUserData.UserInfoID = sysRoleUserInfo.UserInfoID;
            sysRoleUserData.Ids = sysRoleUserInfo.Ids;
            return sysUserInfoDomain.SaveRoleUser(sysRoleUserData);
        }

        public string DelAuList(SysRoleUserInfo stockOutInfo)
        {
            SysRoleUserData sysRoleUserData = new SysRoleUserData();
            sysRoleUserData.Ids = stockOutInfo.Ids;
            //stockOutdata.ID = stockOutInfo.ID;
            return sysUserInfoDomain.DelAuList(sysRoleUserData);
        }

        public string GetLoginUserId()
        {
            return sysUserInfoDomain.GetLoginUserId();
        }
    }
}
