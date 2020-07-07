using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.MasterData;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;

namespace Blocks.BussnessApplicationModule.MasterData
{
    public interface IComboboxAppService : IAppService
    {
        PageList<ComboboxData>  GetComboboxList(SearchModel a);
 
    }
}