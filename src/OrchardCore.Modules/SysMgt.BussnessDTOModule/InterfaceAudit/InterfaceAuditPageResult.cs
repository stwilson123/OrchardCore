using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.InterfaceAudit
{
	public class InterfaceAuditPageResult
	{
		public string Id { get; set; }
		public string MethodName { set; get; }
		public object Parameters { set; get; }
		public DateTime? ExecutionTime { set; get; }
		public string Creater { set; get; }
		public string SysException { set; get; }
		public string MethodDescription { get; set; }
		public object OutParameters { get; set; }
		public string UserAccount { get; set; }
	}
}
