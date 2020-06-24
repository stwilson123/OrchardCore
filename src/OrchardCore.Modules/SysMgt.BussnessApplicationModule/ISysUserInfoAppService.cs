using BlocksCore.Application.Abstratctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule;
using SysMgt.BussnessDTOModule.SysUserInfo;
using SysMgt.BussnessDTOModule.Combobox;
using BlocksCore.Abstractions.UI.Combobox;
using SysMgt.BussnessDTOModule.SysRoleInfo;

namespace SysMgt.BussnessApplicationModule
{
    public interface ISysUserInfoAppService:IAppService
    {
        string Add(SysUserInfo sysUserInfo);
        string Update(SysUserInfo sysUserInfo);
        string PasswordModification(SysUserPwdModificationInfo sysUserInfo);
        string PasswordReset(SysUserInfo SysUserInfo);
        string Disable(SysUserInfo sysUserInfo);
        string Enable(SysUserInfo sysUserInfo);
        SysUserInfo GetOneById(SysUserInfo sysUserInfo);
        PageList<SysUserInfoPageResult> GetPageList(SysUserInfoSearchModel search);
        List<SysUserInfoPageResult> GetRoleUserList(SysUserInfoSearchModel search);
        PageList<SysUserInfoPageResult> GetRoleAuList(SysUserInfoSearchModel search);
        string Allot(AllotInfo allotInfo);
        PageList<ComboboxData> GetComboxList(SearchModel search);
        string Login(SysLoginInfo sysLoginInfo);
        SysRoleUserInfo GetSysRoleByUser(SysUserInfo sysUserInfo);
        string SaveRoleUser(SysRoleUserInfo sysRoleUserInfo);
        string DelAuList(SysRoleUserInfo stockOutInfo);
        string GetLoginUserId();
        string validatorPassword(SysUserPwdModificationInfo sysUserInfo);
    }
}
