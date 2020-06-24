using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions.Domain;
using BlocksCore.Localization.Abtractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using SysMgt.BussnessDTOModule.InterfaceAudit;
using SysMgt.BussnessRespositoryModule.InterfaceAudit;

namespace SysMgt.BussnessDomainModule.InterfaceAudit
{
	public class InterfaceAuditDomain : IDomainService
	{
		public IInterfaceAuditRepository _interfaceAuditRepository { get; set; }

		public PageList<InterfaceAuditPageResult> GetPageList(InterfaceAuditSearchModel search)
		{
			return _interfaceAuditRepository.GetPageList(search);
		}

		public string Add(InterfaceAuditInfo item)
		{
			BLOCKS_AUDITLOGS obj = new BLOCKS_AUDITLOGS()
			{
				Id = Guid.NewGuid().ToString(),
				MethodName = item.MethodName,
				PARAMETERS = item.Parameters,
				ExecutionTime = item.ExecutionTime,
				UserId = item.UserId,
				METHODDESCRIPTION = item.MethodDescription,
				OUTPARAMETERS = item.OutParameters,
				USERACCOUNT = item.UserAccount,
				OUTPARAMETERSDESCRIPTION = item.OutParametersDescription,
				PARAMETERSDESCRIPTION = item.ParametersDescription
			};
			if (string.IsNullOrEmpty(item.Exception))
			{
				obj.EXCEPTION = item.Exception;
			}
			if (string.IsNullOrEmpty(item.SystemException))
			{
				obj.SYSTEMEXCEPTION = item.SystemException;
			}
			string returnId = _interfaceAuditRepository.InsertAndGetId(obj);
			if (string.IsNullOrEmpty(returnId))
			{
				return "新增失败";
			}
			else
			{
				return "新增成功";
			}
		}
	}
}
