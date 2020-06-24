using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Combobox;

namespace SysMgt.BussnessApplicationModule
{
    public interface IDictionaryAppService : IAppService
    {
        PageList<ComboboxData> GetComboxList(SearchModel search);
    }
}
