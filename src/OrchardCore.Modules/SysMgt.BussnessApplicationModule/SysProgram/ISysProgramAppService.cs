using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.SysProgram;

namespace SysMgt.BussnessApplicationModule.SysProgram
{
   public interface ISysProgramAppService : IAppService
   {
       void Bind(BindSysProgramInfo bindSysProgramInfo);
        PageList<SysProgramPageResult> GetPageList(SysProgramSearchModel search);
    }
}
