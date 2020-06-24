 using BlocksCore.Abstractions.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SysMgt.BussnessDTOModule.InterfaceAudit;
using BlocksCore.Event.Abstractions;
using BlocksCore.Audit.Event;

namespace SysMgt.BussnessDomainModule.InterfaceAudit
{
	//[UnitOfWork(TransactionScopeOption.RequiresNew)]
	public class InterfaceAuditInitEventHandler : IEventHandler<AuditSaveEventData>
	{
		public InterfaceAuditDomain _interfaceDomain { get; set; }
		private IUserContext _userContext { get; set; }

		public InterfaceAuditInitEventHandler(InterfaceAuditDomain interfaceDomain, IUserContext userContext)
		{
			_interfaceDomain = interfaceDomain;
			_userContext = userContext;
		}

		public virtual void HandleEvent(AuditSaveEventData eventData)
		{
			if (!string.IsNullOrEmpty(eventData.Data.MethodDescription))
			{
				InterfaceAuditInfo item = new InterfaceAuditInfo()
				{
					MethodName = eventData.Data.MethodName,
					Parameters = eventData.Data.Parameters,
					ExecutionTime = eventData.Data.ExecutionTime,
					UserId = eventData.Data.UserId,
					MethodDescription = eventData.Data.MethodDescription,
					OutParameters = eventData.Data.OutParameters,
					UserAccount = eventData.Data.UserAccount,
					OutParametersDescription = eventData.Data.OutParametersDescription,
					ParametersDescription = eventData.Data.ParametersDescription
				};
				if (eventData.Data.Exception != null)
				{
					item.Exception = eventData.Data.Exception.StackTrace + eventData.Data.Exception.Message;
				}
				if (eventData.Data.SystemException != null)
				{
					item.SystemException = eventData.Data.SystemException.StackTrace + eventData.Data.SystemException.Message;
				}
				_interfaceDomain.Add(item);
			}
		}
	}
}