using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.Abstractions.UnitOfWork;

using BlocksCore.Data.Linq;
using BlocksCore.Data.Linq2DB.Repository;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.SysRoleInfo;

namespace SysMgt.BussnessRespositoryModule
{
   public class SysRoleInfoRepository : DBSqlRepositoryBase<SYS_ROLEINFO>, ISysRoleInfoRepository
    {
        public SysRoleInfoRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
            
        }

        public PageList<SysRoleInfoPageResult> GetPageList(SysRoleInfoSearchModel search)
        {
            
            return GetContextTable().Paging((SYS_ROLEINFO t) => new SysRoleInfoPageResult()
            {
                ID = t.Id,
                Name = t.CNAME,
                Remark = t.MEMO
            }, search.page);
        }
        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return GetContextTable().Paging((SYS_ROLEINFO t) => new ComboboxData
            {
                Id = t.Id,
                Text = t.CNAME
            }, search.page);
        }

        public PageList<SysRoleInfoPageResult> GetUserAuList(SysRoleInfoSearchModel search)
        {

            var userInfos = GetContextTable()                
                .InnerJoin((SYS_ROLEINFO sys) => sys.Id, (SYS_ROLEUSER s) => s.SYS_ROLEINFOID)
                //.Distinct()
                ;
           
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.UserId))
                {
                    userInfos = userInfos.Where((SYS_ROLEUSER s) => s.SYS_USERINFOID == search.UserId);
                }

            }

            return userInfos.Paging((SYS_ROLEUSER s, SYS_ROLEINFO sys) => new SysRoleInfoPageResult()
            {
                ID = sys.Id,
                Name = sys.CNAME,
                Remark = sys.MEMO
            }, search.page);
        }
    }
}
