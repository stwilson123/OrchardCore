using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.MasterData;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;

namespace Blocks.BussnessApplicationModule.MasterData
{
    public interface IMasterDataAppService : IAppService
    {
        PageList<PageResult>  GetPageList(SearchModel a);

        void Add(MasterDataInfo masterDataInfo);

        void TestException();

        string ProxTest();
        MasterDataInfo Get(MasterDataInfo info);
    }
}