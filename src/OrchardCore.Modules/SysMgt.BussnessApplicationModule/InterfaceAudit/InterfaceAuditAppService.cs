using BlocksCore.Application.Abstratctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysMgt.BussnessDomainModule.InterfaceAudit;
using BlocksCore.Abstractions.Security;
using SysMgt.BussnessDTOModule.InterfaceAudit;
using BlocksCore.Data.Abstractions.Paging;

namespace SysMgt.BussnessApplicationModule.InterfaceAudit
{
	public class InterfaceAuditAppService : IAppService, IInterfaceAuditAppService
	{
		public InterfaceAuditDomain _interfaceDomain { get; set; }
		private IUserContext _userContext { get; set; }

		public InterfaceAuditAppService(InterfaceAuditDomain interfaceDomain, IUserContext userContext)
		{
			_interfaceDomain = interfaceDomain;
			_userContext = userContext;
		}

		public PageList<InterfaceAuditPageResult> GetPageList(InterfaceAuditSearchModel search)
		{
			return _interfaceDomain.GetPageList(search);
		}
	}
}
