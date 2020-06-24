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
using SysMgt.BussnessDTOModule.ProductFormat;

namespace SysMgt.BussnessRespositoryModule
{
   public interface IProductFormatRepository: IRepository<BDTA_PRODUCTFORMAT>
    {
        PageList<ProductFormatPageResult> GetPageList(ProductFormatSearchModel search);
        PageList<ComboboxData> GetComboxList(SearchModel search);
    }
}
