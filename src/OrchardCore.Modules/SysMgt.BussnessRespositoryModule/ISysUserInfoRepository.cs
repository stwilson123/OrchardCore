using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using SysMgt.BussnessDTOModule.SysUserInfo;
using SysMgt.BussnessDTOModule.Combobox;
using BlocksCore.Abstractions.UI.Combobox;

namespace SysMgt.BussnessRespositoryModule
{
    public interface ISysUserInfoRepository: IRepository<SYS_USERINFO>
    {
        PageList<SysUserInfoPageResult> GetPageList(SysUserInfoSearchModel search);
        PageList<SysUserInfoPageResult> GetRoleAuList(SysUserInfoSearchModel search);
        PageList<ComboboxData> GetComboxList(SearchModel search);
        List<SysUserRoles> GetUserRoles(SysUserRolesSearchModel searchModel);
    }
}
