using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.Department;
using SysMgt.BussnessDTOModule;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.Common;

namespace SysMgt.BussnessApplicationModule
{
    public interface IDepartmentAppService : IAppService
    {
        PageList<DepartmentPageResult> GetPageList(DepartmentSearchModel search);
        string Add(DepartmentInfo setupInfo);
        string Update(DepartmentInfo outInTypeInfo);
        string Delete(CommonEntity entity4Delete);
        DepartmentData GetOneById(DepartmentInfo outInTypeInfo);

        PageList<ComboboxData> GetComboxList(SearchModel search);

    }
}
