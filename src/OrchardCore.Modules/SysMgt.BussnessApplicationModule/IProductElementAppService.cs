using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.ProductElementType;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.ProductElement;
using SysMgt.BussnessDTOModule.ProductElementType;
using SysMgt.BussnessDTOModule.SysActionType;

namespace SysMgt.BussnessApplicationModule
{
    public interface IProductElementAppService : IAppService
    {

        string Add(ProductElementInfo productElementInfo);
        string Delete(ProductElementInfo productElementInfo);

        string Update(ProductElementInfo productElementInfo);

        PageList<ProductElementPageResult> GetPageList(ProductElementSearchModel search);

        ProductElementInfo GetOneById(ProductElementInfo productElementInfo);

        List<ProductElementInfo> GetAllList();

        PageList<ComboboxData> GetComboxList(SearchModel search);
    }
}
