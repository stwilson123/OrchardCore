using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.Employee;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.Employee;


namespace SysMgt.BussnessApplicationModule
{
    public interface IEmployeeAppService : IAppService
    {

        PageList<EmployeePageResult> GetPageList(EmployeeSearchModel search);
        string Add(EmployeeInfo setupInfo);
        string Update(EmployeeInfo outInTypeInfo);
        string Delete(CommonEntity entity4Delete);
        EmployeeInfo GetOneById(EmployeeInfo outInTypeInfo);
        PageList<ComboboxData> GetComboxList(SearchModel search);
    }
}
