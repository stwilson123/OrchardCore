using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.ProductFormat;

namespace SysMgt.BussnessApplicationModule
{
    public interface IProductFormatAppService:IAppService
    {
        string Add(ProductFormatInfo productFormatInfo);
        string Delete(ProductFormatInfo productFormatInfo);
        string Update(ProductFormatInfo productFormatInfo);

        ProductFormatInfo GetOneById(ProductFormatInfo productFormatInfo);
        PageList<ProductFormatPageResult> GetPageList(ProductFormatSearchModel search);

        PageList<ComboboxData> GetComboxList(SearchModel search);

        List<string> GetNumbers(CodeElementInfo codeElementInfo);
    }
}
