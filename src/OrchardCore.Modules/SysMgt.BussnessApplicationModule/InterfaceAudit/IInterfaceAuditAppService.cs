using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.InterfaceAudit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessApplicationModule.InterfaceAudit
{
	public interface IInterfaceAuditAppService : IAppService
	{
		PageList<InterfaceAuditPageResult> GetPageList(InterfaceAuditSearchModel search);
	}
}
