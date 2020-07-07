using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Localization.Abtractions;
using BlocksCore.Abstractions.Datatransfer;

namespace SysMgt.BussnessDTOModule.SysProgram
{
	public class BindSysProgramInfo 
	{
		[LocalizedDescription("ID")]
		public string ID { get; set; }
		[LocalizedDescription("LIST_OF_DETAILS")]
		public List<SysProgramInfo> ListsSysProgramInfos { get; set; }
		public int Platform { get; set; }
	}
}
