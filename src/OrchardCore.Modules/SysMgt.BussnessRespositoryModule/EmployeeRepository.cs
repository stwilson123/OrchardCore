using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Repository;
using SysMgt.BussnessDTOModule;
using SysMgt.BussnessDTOModule.Employee;
using BlocksCore.Data.EF.Linq;
using BlocksCore.Abstractions.UI.Combobox;
using SysMgt.BussnessDTOModule.Combobox;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule
{
    public class EmployeeRepository : DBSqlRepositoryBase<BDTA_EMPLOYEE>, IEmployeeRepository
    {
        public EmployeeRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        public PageList<EmployeePageResult> GetPageList(EmployeeSearchModel search)
        {
            return GetContextTable()
                //.LeftJoin((BDTA_EMPLOYEE t) => t.DEPARTMENT_ID, (BDTA_DEPARTMENT b) => b.Id)
                .Paging((BDTA_EMPLOYEE t) => new EmployeePageResult
                {
                    ID = t.Id,
                    Code = t.EMPLOYEE_NO,
                    Name = t.EMPLOYEE_NAME,
                    Desc = t.EMPLOYEE_DESC,
                    EmpType = t.EMPLOYEE_TYPE,
                    Email = t.EMPLOYEE_EMAIL,
                    Phone = t.EMPLOYEE_PHONE,
                    DeptName = t.BDTA_DEPARTMENT.DEPARTMENT_NAME
                }, search.page);


            //return GetContextTable()               
            //    .Paging((BDTA_EMPLOYEE t) => new EmployeeModel4PageResult
            //    {
            //        ID = t.Id,
            //        Code = t.EMPLOYEE_NO,
            //        Name = t.EMPLOYEE_NAME,
            //        Desc = t.EMPLOYEE_DESC,
            //        EmpType = t.EMPLOYEE_TYPE,
            //        Email = t.EMPLOYEE_EMAIL,
            //        Phone = t.EMPLOYEE_PHONE,                   
            //        DeptName = t.BDTA_DEPARTMENT.DEPARTMENT_NAME
            //    }, search.page);


        }



        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return GetContextTable().Paging((BDTA_EMPLOYEE t) => new ComboboxData
            {
                Id = t.Id,
                Text = t.EMPLOYEE_NAME
            }, search.page);
        }


    }
}
