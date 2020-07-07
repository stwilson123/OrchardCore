using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.SysProgram
{
	public class SysProgramSearchModel 
	{
		[DataTransfer("page")]
		public Page page { get; set; }

		public string ProgramParent { get; set; }

		public int Platform { get; set; } = 1;
	}
}
