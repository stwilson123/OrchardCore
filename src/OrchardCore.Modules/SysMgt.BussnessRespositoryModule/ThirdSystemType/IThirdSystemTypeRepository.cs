using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.ThirdSystemType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessRespositoryModule.ThirdSystemType
{ 
    public interface IThirdSystemTypeRepository : IRepository<SYS_THIRD_SYSTEM_TYPE>
    {
        PageList<ThirdSystemTypePageResult> GetPageList(ThirdSystemTypeSearchModel search); 

    }
}
