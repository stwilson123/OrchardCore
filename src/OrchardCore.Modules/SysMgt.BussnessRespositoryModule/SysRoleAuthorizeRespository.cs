using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.UnitOfWork;

using BlocksCore.Data.Linq2DB.Repository;

namespace SysMgt.BussnessRespositoryModule
{
   public class SysRoleAuthorizeRespository : DBSqlRepositoryBase<SYS_ROLEAUTHORIZE>, ISysRoleAuthorizeRespository
    {
        public SysRoleAuthorizeRespository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
            
        }

        public List<SYS_ROLEAUTHORIZE> GetRoleAuthorize(string roleId)
        {
            return GetAllCode().Where(s => s.SYS_ROLEORUSERID == roleId)
                 .Select(s => new SYS_ROLEAUTHORIZE() { SYS_ROLEORUSERID = s.SYS_ROLEORUSERID, RESOURCE_KEY = s.RESOURCE_KEY }).ToList();
        }

        public List<SYS_ROLEAUTHORIZE> GetRoleAuthorizes()
        {
            return GetAllCode()
                  .Select(s => new SYS_ROLEAUTHORIZE() { SYS_ROLEORUSERID = s.SYS_ROLEORUSERID, RESOURCE_KEY = s.RESOURCE_KEY }).ToList();
        }
    }
}
