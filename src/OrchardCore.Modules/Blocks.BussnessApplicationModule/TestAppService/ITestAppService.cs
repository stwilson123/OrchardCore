using System;
using System.Collections.Generic;
using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.MasterData;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;

namespace Blocks.BussnessApplicationModule.TestAppService
{
	public interface ITestAppService : IAppService
	{
		string GetValue(string a);

		void Add(MasterDataInfo masterDataInfo);

		List<string> ProxFunction(ProxModel input);

		string GetValueOverride();

		object GetData(SearchModel page);

		PageList<PageResult> GetServerData(SearchModel page);

		object GetClientData(SearchModel page);
	}
}