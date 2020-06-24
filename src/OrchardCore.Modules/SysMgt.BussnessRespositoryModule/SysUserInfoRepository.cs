using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Repository;
using SysMgt.BussnessDTOModule;
using BlocksCore.Data.EF.Linq;
using Blocks.BussnessEntityModule;
using SysMgt.BussnessDTOModule.SysUserInfo;
using BlocksCore.Abstractions.UI.Combobox;
using SysMgt.BussnessDTOModule.Combobox;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule
{
    public class SysUserInfoRepository: DBSqlRepositoryBase<SYS_USERINFO>, ISysUserInfoRepository
    {
        public SysUserInfoRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }
        public List<SysUserRoles> GetUserRoles(SysUserRolesSearchModel searchModel)
        {
            var userInfos = GetContextTable().Where((SYS_USERINFO sys) => sys.USERCODE == searchModel.UserCode);

            return userInfos.SelectToList((SYS_USERINFO sys) => new SysUserRoles()
            {
                USERCODE = sys.USERCODE,
                USERID = sys.Id,
                UserRoles = sys.SYS_ROLEUSERs.Select(i => i.SYS_ROLEINFOID).ToList()
            });
        }
        public PageList<SysUserInfoPageResult> GetPageList(SysUserInfoSearchModel search)
        { 
            var list = GetContextTable();
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.State))
                {
                    long s = long.Parse(search.State);
                    list = list.Where((SYS_USERINFO sys) => sys.STATE == s);
                } 
            }

            return list.Paging((SYS_USERINFO sys) => new SysUserInfoPageResult()
            { 
                ID = sys.Id,
                CName = sys.CNAME,
                UserCode = sys.USERCODE,

                Memo = sys.MEMO,
                State = sys.STATE.ToString(),
            }, search.page); 
        }
         


        public PageList<SysUserInfoPageResult> GetRoleAuList(SysUserInfoSearchModel search)
        {
            var userInfos = GetContextTable()
                 .InnerJoin((SYS_USERINFO sys) => sys.Id, (SYS_ROLEUSER s) => s.SYS_USERINFOID);
                // .Where((SYS_ROLEUSER s) => s.SYS_ROLEINFOID == search.RoleId);
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.RoleId))
                {
                    userInfos = userInfos.Where((SYS_ROLEUSER s) => s.SYS_ROLEINFOID == search.RoleId);
                }
                
            }

            return  userInfos.Paging((SYS_ROLEUSER s ,SYS_USERINFO sys) => new SysUserInfoPageResult()
            {
                ID = sys.Id,
                CName = sys.CNAME,
                UserCode = sys.USERCODE,
                Memo = sys.MEMO,
                State = sys.STATE.ToString(),
            }, search.page);
        }
        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return GetContextTable().Paging((SYS_USERINFO t) => new ComboboxData
            {
                Id = t.Id,
                Text = t.CNAME
            }, search.page);
        }
    }
}
