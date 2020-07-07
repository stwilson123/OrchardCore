using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.MasterData;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.Abstractions.Repository;
using System.Collections.Generic;

namespace Blocks.BussnessRespositoryModule
{
    public interface ITestRepository : IRepository<TESTENTITY>
    {
        string GetValue(string value);

        string GetValueOverride(string value);

        PageList<PageResult> GetPageList(SearchModel search);

        List<PageResult> GetList();
    }
}