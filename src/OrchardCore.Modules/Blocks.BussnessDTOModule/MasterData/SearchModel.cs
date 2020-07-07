
using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Abstractions.Datatransfer;
using BlocksCore.Localization.Abtractions;

namespace Blocks.BussnessDTOModule.MasterData
{
	public class SearchModel 
	{
		[DataTransfer("page")]
		[LocalizedDescription("Page")]
		public Page page { get; set; }
	}

	public class IonicPageModel
	{
		public int pageIndex { get; set; }

		public int pageSize { get; set; }

	}
}