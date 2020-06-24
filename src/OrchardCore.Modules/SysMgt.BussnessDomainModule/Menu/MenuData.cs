using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SysMgt.BussnessDomainModule.Menu
{
	public class MenuData
	{
		public string ID { get; set; }
		public List<string> IDS { get; set; }
		//public string Name { get; set; }
		public string Code { get; set; }
		public string Type { get; set; }
		public string Desc { get; set; }
		public string pId { get; set; }
		public long Sort { get; set; }
		public string Icon { get; set; }
		public string UId { get; set; }
		public string IndexIcon { get; set; }
		public long Platform { get; set; }
	}
}