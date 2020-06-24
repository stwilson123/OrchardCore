using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.ProductElement;

namespace SysMgt.BussnessRespositoryModule
{
    public interface IProduct4VarElementRepository : IRepository<BDTA_PRODUCT_VARELEMENT_REL>
    {

        //PageList<ComboboxData> GetComboxList(SearchModel search);
        //PageList<ProductElementPageResult> GetPageList(ProductElementSearchModel productElementSearchModel);
    }
}
