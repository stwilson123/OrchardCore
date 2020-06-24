using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.SysProgram;
using SysMgt.BussnessDTOModule.SysRoleInfo;
using SysMgt.BussnessDTOModule.SysUserInfo;


namespace SysMgt.BussnessApplicationModule
{
   public interface ISysRoleInfoAppService:IAppService
    {
        PageList<SysRoleInfoPageResult> GetPageList(SysRoleInfoSearchModel search);
        PageList<SysRoleInfoPageResult> GetUserAuList(SysRoleInfoSearchModel search);

        List<SysRoleInfoPageResult> GetUserNotRoleList(SysRoleInfoSearchModel search);
        string Add(SysRoleInfo sysRoleInfo);

        string Delete(SysRoleInfo sysRoleInfo);
        string Edit(SysRoleInfo sysRoleInfo);
        void Allot(SysRoleInfo sysRoleInfo);
        SysRoleInfo GetOneById(SysRoleInfo sysRoleInfo);

        PageList<ComboboxData> GetComboxList(SearchModel search);
        List<SysRoleInfoinfo> GetALLList(SearchModel search);
        List<SysPogramTree> GetAllSysProgram(SysRoleInfo sysRoleInfo);
        ELsysPogramTreeCheckedNode GetAllELSysProgram(SysRoleInfo sysRoleInfo);

        SysRoleAndUserInfo GetSysUserByRole(SysRoleInfo sysRoleInfo);

        string SaveRoleAndUser(SysRoleAndUserInfo sysRoleAndUserInfo);

        string SaveRoleMenu(SysRoleAndUserInfo sysRoleAndUserInfo);
        string DelAuList(SysRoleAndUserInfo sysRoleAndUserInfo);
    }
}
