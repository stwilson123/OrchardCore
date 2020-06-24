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
    public interface IProductElementRepository : IRepository<BDTA_PRODUCTELEMENT>
    {

        PageList<ComboboxData> GetComboxList(SearchModel search);
        PageList<ProductElementPageResult> GetPageList(ProductElementSearchModel productElementSearchModel);
    }
}
