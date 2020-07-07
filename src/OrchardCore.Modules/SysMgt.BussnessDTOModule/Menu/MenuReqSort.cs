using BlocksCore.Abstractions.Datatransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.Menu
{
	public class MenuReqSort 
	{
		public string id { get; set; }

		public string targetId { get; set; }

		public string moveType { get; set; }
	}

	public class MenuSortDTO
	{
		public MenuSortDTO()
		{
			success = false;
			msg = "";
		}
		public bool success { get; set; }
		public string msg { get; set; }
	}

	public class MenuTempDTO
	{
		public string Id { get; set; }

		public decimal Sort { get; set; }
	}
}