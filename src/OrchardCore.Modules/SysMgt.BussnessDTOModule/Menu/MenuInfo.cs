using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BlocksCore.Localization.Abtractions;
using BlocksCore.Application.Abstratctions.Datatransfer;

namespace SysMgt.BussnessDTOModule.Menu
{
	public class MenuInfo 
	{
		[LocalizedDescription("ID")]
		public string ID { get; set; }
		[LocalizedDescription("Ids")]
		public List<string> IDS { get; set; }
		//public string Name { get; set; }
		[LocalizedDescription("CODE")]
		public string Code { get; set; }
		public string Type { get; set; }
		[LocalizedDescription("REMARK")]
		public string Desc { get; set; }

		public string PId { get; set; }
		[LocalizedDescription("ICON")]
		public string Icon { get; set; }

		public long Sort { get; set; }
		public string UId { get; set; }
		public string IndexIcon { get; set; }

		public long Platform { get; set; } = 1;
	}

	public class VisitMenu 
	{
		public string MenuID { get; set; }
		public string MenuCode { get; set; }
		public string MenuIcon { get; set; }
        //public ILocalizableString MenuName { get; set; }
        public string MenuName { get; set; }
        public long Seq { get; set; }

	}
}