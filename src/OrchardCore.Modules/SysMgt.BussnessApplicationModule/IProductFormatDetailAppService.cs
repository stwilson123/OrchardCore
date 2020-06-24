using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.ProductFormatDetail;

namespace SysMgt.BussnessApplicationModule
{
   public interface IProductFormatDetailAppService:IAppService
    {
        PageList<ProductFormatDetailPageResult> GetPageList(ProductFormatDetailSearchModel search);
    }
}
