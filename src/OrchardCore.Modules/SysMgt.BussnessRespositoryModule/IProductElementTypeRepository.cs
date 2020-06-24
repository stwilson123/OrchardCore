using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.ProductElementType;

namespace SysMgt.BussnessRespositoryModule
{
   public interface IProductElementTypeRepository:IRepository<BDTA_PRODUCTELEMENT_TYPE>
   {
       PageList<ProductElementTypePageResult> GetPageList(ProductElementTypeSearchModel search);

       PageList<ComboboxData> GetComboxList(ProductElementTypeSearchModel search);
   }
} 
