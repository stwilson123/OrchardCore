using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;

namespace SysMgt.BussnessRespositoryModule
{
    public interface ISysRoleAuthorizeRespository : IRepository<SYS_ROLEAUTHORIZE>
    {
        List<SYS_ROLEAUTHORIZE> GetRoleAuthorize(string roleId);

        List<SYS_ROLEAUTHORIZE> GetRoleAuthorizes();

    }
}
