using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.ProductFormatDetail;

namespace SysMgt.BussnessRespositoryModule
{
  public  interface IProductFormatDetailRepository : IRepository<BDTA_PRODUCTFORMAT_DETAIL>
  {
      PageList<ProductFormatDetailPageResult> GetPageList(ProductFormatDetailSearchModel search);
        //object GetListByFormatId(string id);

        List<ProductFormatDetailInfo> GetListByFormatId(string formatId);
    }
}
