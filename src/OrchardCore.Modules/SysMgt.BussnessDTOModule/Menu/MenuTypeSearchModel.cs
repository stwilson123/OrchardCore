using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Application.Abstratctions.Datatransfer;

namespace SysMgt.BussnessDTOModule.Menu
{

	public class MenuTypeSearchModel 
	{										  
		public int platform { get; set; } = 1;

		[DataTransfer("page")]
		public Page page { get; set; }

		public string ID { get; set; }
	}
}
