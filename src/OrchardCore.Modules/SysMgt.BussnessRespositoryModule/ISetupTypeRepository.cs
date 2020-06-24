using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessRespositoryModule
{

    public interface ISetupTypeRepository : IRepository<BDTA_SETUP_TYPE>
    {
        PageList<SetupTypePageResult> GetPageList(SetupTypeSearchModel search);
    }
}
