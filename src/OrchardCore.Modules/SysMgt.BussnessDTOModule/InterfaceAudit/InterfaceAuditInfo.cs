using BlocksCore.Application.Abstratctions.Datatransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.InterfaceAudit
{
	public class InterfaceAuditInfo 
	{
		public string MethodName { set; get; }
		public string Parameters { set; get; }
		public DateTime ExecutionTime { get; set; }
		public long? UserId { set; get; }
		public string Exception { set; get; }
		public string MethodDescription { get; set; }
		public string OutParameters { get; set; }
		public string UserAccount { get; set; }
		public string OutParametersDescription { get; set; }
		public string ParametersDescription { get; set; }
		public string SystemException { get; set; }
	}
}
