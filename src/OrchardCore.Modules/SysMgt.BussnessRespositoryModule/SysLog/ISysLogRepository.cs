using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessRespositoryModule.SysLog
{ 
    public interface ISysLogRepository : IRepository<SYS_LOG>
    {
        //PageList<ThirdSystemTypePageResult> GetPageList(ThirdSystemTypeSearchModel search); 

    }
}
