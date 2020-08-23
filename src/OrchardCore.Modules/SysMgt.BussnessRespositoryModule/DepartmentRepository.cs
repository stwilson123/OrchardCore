using BlocksCore.Data.Abstractions.Paging;

using BlocksCore.Data.Linq2DB.Repository;
using SysMgt.BussnessDTOModule;
using BlocksCore.Data.Linq;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using SysMgt.BussnessDTOModule.Combobox;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule
{
    public class DepartmentRepository : DBSqlRepositoryBase<BDTA_DEPARTMENT>, IDepartmentRepository
    {

        public DepartmentRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }


        public PageList<DepartmentPageResult> GetPageList(DepartmentSearchModel search)
        {
            return GetContextTable().Paging((BDTA_DEPARTMENT t) => new DepartmentPageResult
            {
                ID = t.Id,
                Code = t.DEPARTMENT_NO,
                Name = t.DEPARTMENT_NAME,
                Desc=t.DEPARTMENT_DESC
            }, search.page);
        }

        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return GetContextTable().Paging((BDTA_DEPARTMENT t) => new ComboboxData
            {
                Id = t.Id,
                Text = t.DEPARTMENT_NAME 
            }, search.page);
        }
    }
}
