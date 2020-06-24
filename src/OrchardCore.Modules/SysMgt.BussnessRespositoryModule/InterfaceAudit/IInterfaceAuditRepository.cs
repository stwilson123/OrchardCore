using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.InterfaceAudit;

namespace SysMgt.BussnessRespositoryModule.InterfaceAudit
{
	public interface IInterfaceAuditRepository : IRepository<BLOCKS_AUDITLOGS>
	{
		PageList<InterfaceAuditPageResult> GetPageList(InterfaceAuditSearchModel search);
	}
}
