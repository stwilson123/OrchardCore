
using BlocksCore.Application.Abstratctions;
using BlocksCore.AutoMapper.Abstractions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.Department;
using SysMgt.BussnessDTOModule;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.Common;


namespace SysMgt.BussnessApplicationModule
{
    public class DepartmentAppService : AppService, IDepartmentAppService
    {

        private DepartmentDomain departmentDomain { get; set; }
        public DepartmentAppService(DepartmentDomain departmentDomain)
        {
            this.departmentDomain = departmentDomain;
        }
        public PageList<DepartmentPageResult> GetPageList(DepartmentSearchModel search)
        {
            return departmentDomain.GetPageList(search);
        }
        public string Add(DepartmentInfo model)
        {
            DepartmentData departmentData = new DepartmentData();
            departmentData.Code = model.Code;
            departmentData.Name = model.Name;
            departmentData.Desc = model.Desc;
            return departmentDomain.Add(departmentData);
        }
        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return departmentDomain.GetComboxList(search);
        }

        public string Update(DepartmentInfo departmentInfo)
        {
            DepartmentData departmentData = departmentInfo.AutoMapTo<DepartmentData>();
            return departmentDomain.Update(departmentData);
        }

        public string Delete(CommonEntity entity4Delete)
        {           
            return departmentDomain.Delete(entity4Delete.IDs);
        }

        public DepartmentData GetOneById(DepartmentInfo departmentInfo)
        {
            DepartmentData departmentData = departmentInfo.AutoMapTo<DepartmentData>();
            return departmentDomain.GetOneById(departmentData);
        }
    }
}