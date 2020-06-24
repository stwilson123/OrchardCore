using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.Employee;

namespace SysMgt.BussnessRespositoryModule
{
    public interface IEmployeeRepository : IRepository<BDTA_EMPLOYEE>
    {
        PageList<EmployeePageResult> GetPageList(EmployeeSearchModel search);
        PageList<ComboboxData> GetComboxList(SearchModel search);
    }
}
