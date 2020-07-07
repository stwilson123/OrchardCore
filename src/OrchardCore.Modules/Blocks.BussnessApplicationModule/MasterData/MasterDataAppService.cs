using System;
using Blocks.BussnessDomainModule;
using Blocks.BussnessDomainModule.MasterData;
using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.MasterData;
using BlocksCore.Application.Abstratctions;
using BlocksCore.AutoMapper.Abstractions;
using BlocksCore.Data.Abstractions.Paging;
using Microsoft.Extensions.Localization;
using BlocksCore.Abstractions.Security;
using BlocksCore.Domain.Abstractions;


namespace Blocks.BussnessApplicationModule.MasterData
{
	public class MasterDataAppService : AppService, IMasterDataAppService
	{
		private readonly ActualMasterData _actualMasterData;

        public MasterDataAppService(MasterDataDomainEvent masterDataDomain, ActualMasterData actualMasterData, IStringLocalizer<MasterDataAppService> l)
        {
            _actualMasterData = actualMasterData;
            this.masterDataDomain = masterDataDomain;
            L = l;
            //_userContext = userContext;
        }

        public IUserContext UserContext { get; set; }
		public IStringLocalizer L { get; set; }

		private MasterDataDomainEvent masterDataDomain { get; set; }


		//[LocalizedDescription("ApiNameKey")]
		public PageList<PageResult> GetPageList(SearchModel a)
		{
			return masterDataDomain.GetPageList(a);
		}

		public MasterDataInfo Get(MasterDataInfo masterDataInfo)
		{
			var result = masterDataDomain.Get(masterDataInfo.Id);

			return result.AutoMapTo<MasterDataInfo>();
		}

		public void Add(MasterDataInfo masterDataInfo)
		{
			var result = masterDataDomain.Add(masterDataInfo.AutoMapTo<BussnessDomainModule.MasterData.MasterData>());
		}

		public void TestException()
		{
			var lException = L["TestException"];
			_actualMasterData.TestException();
			masterDataDomain.TestException();
			throw new BlocksBussnessException("101", L["TestException"], null);
		}

		public string ProxTest()
		{
            return null;
			//return masterDataDomain.ProxTest("abc");
		}
	}
}