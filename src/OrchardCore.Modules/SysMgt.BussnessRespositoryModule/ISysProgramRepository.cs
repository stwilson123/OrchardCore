using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.SysProgram;

namespace SysMgt.BussnessRespositoryModule
{
	public interface ISysProgramRepository : IRepository<SYS_PROGRAM>
	{
		PageList<SysProgramPageResult> GetPageList(SysProgramSearchModel search);
	}
}
