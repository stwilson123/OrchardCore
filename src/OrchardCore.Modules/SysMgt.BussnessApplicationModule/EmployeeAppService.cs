using BlocksCore.Application.Abstratctions;
using BlocksCore.AutoMapper.Abstractions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.Employee;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.Employee;

namespace SysMgt.BussnessApplicationModule
{
    public class EmployeeAppService : IAppService, IEmployeeAppService
    {
        private EmployeeDomain employeeDomain { get; set; }

        public EmployeeAppService(EmployeeDomain employeeDomain)
        {
            this.employeeDomain = employeeDomain;
        }

        public PageList<EmployeePageResult> GetPageList(EmployeeSearchModel search)
        {
            PageList<EmployeePageResult> employeePageResults = employeeDomain.GetPageList(search);

            return employeePageResults;
        }

        public string Add(EmployeeInfo employeeInfo)
        {

            EmployeeData employeeData = employeeInfo.AutoMapTo<EmployeeData>();
            return employeeDomain.Add(employeeData);

        }

        public string Update(EmployeeInfo employeeInfo)
        {
            EmployeeData employeeData = employeeInfo.AutoMapTo<EmployeeData>();
            return employeeDomain.Update(employeeData);
        }

        public string Delete(CommonEntity entity4Delete)
        {
            return employeeDomain.Delete(entity4Delete.IDs);
        }

        public EmployeeInfo GetOneById(EmployeeInfo employeeInfo)
        {
            EmployeeData employeeData = employeeInfo.AutoMapTo<EmployeeData>();
            employeeData = employeeDomain.GetOneById(employeeData);
            return employeeData.AutoMapTo<EmployeeInfo>();
        }
        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return employeeDomain.GetComboxList(search);
        }

    }
}