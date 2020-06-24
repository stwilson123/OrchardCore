using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.ProductElementType;
using SysMgt.BussnessDTOModule.ProductElementType;

namespace SysMgt.BussnessApplicationModule
{
   public interface IProductElementTypeAppService:IAppService
   {
       string Add(ProductElementTypeInfo productElementTypeInfo);

       string Delete(ProductElementTypeInfo productElementTypeInfo);

       string Update(ProductElementTypeInfo productElementTypeInfo);
       ProductElementTypeInfo GetOneById(ProductElementTypeInfo productElementTypeInfo);
       PageList<ProductElementTypePageResult> GetPageList(ProductElementTypeSearchModel search);

       PageList<ComboboxData> GetComboxList(ProductElementTypeSearchModel search);
    }
}
