using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule;
using SysMgt.BussnessDTOModule.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessRespositoryModule
{
    public interface IDepartmentRepository : IRepository<BDTA_DEPARTMENT>
    {

        PageList<DepartmentPageResult> GetPageList(DepartmentSearchModel search);

        PageList<ComboboxData> GetComboxList(SearchModel search);

    }
}
