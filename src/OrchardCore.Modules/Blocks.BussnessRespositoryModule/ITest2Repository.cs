using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.Combobox;
using Blocks.BussnessDTOModule.MasterData;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.Abstractions.Repository;

namespace Blocks.BussnessRespositoryModule
{
    public interface ITest2Repository : IRepository<TESTENTITY2>
    {

        PageList<ComboboxData> GetPageList(SearchModel search);
    }
}