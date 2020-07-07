using System;

namespace Blocks.BussnessDTOModule.MasterData
{
	public class MasterDataInfo 
	{
		public string Id { get; set; }
		public string tenancyName { get; set; }

		public string city { get; set; }

		public string combobox { get; set; }

		public bool isActive { get; set; }

		public string comment { get; set; }

		public DateTime registerTime { get; set; }
	}

	public class TestData
	{
		public string ID { get; set; }

		public string name { get; set; }

		public int age { get; set; }

		public string theDate { get; set; }

		public string theSelect { get; set; }
	}
}