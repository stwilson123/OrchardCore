using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.Setup;

namespace SysMgt.BussnessRespositoryModule
{
    public interface ISetupRepository: IRepository<BDTA_SETUP>
    {
        PageList<SetupPageResult> GetPageList(SetupSearchModel search);
        PageList<ComboboxData> GetComboxListByKey(SearchModel search,string key);
        PageList<ComboboxData> GetComboxListByType(SearchModel search, string type);
    }
}
