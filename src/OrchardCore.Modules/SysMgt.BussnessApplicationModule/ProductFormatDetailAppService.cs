//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BlocksCore.Application.Abstratctions;
//using BlocksCore.Data.Abstractions.Paging;
//using SysMgt.BussnessDomainModule;
//using SysMgt.BussnessDTOModule.ProductFormatDetail;

//namespace SysMgt.BussnessApplicationModule
//{
//   public class ProductFormatDetailAppService: AppService, IProductFormatDetailAppService
//    {
//        public ProductFormatDetailDomain productFormatDetailDomain { get; set; }
//        public ProductFormatDetailAppService(ProductFormatDetailDomain productFormatDetailDomain)
//        {
//            this.productFormatDetailDomain = productFormatDetailDomain;
//        }

//        public PageList<ProductFormatDetailPageResult> GetPageList(ProductFormatDetailSearchModel search)
//        {
//            return productFormatDetailDomain.GetPageList(search);
//        }
//    }
//}
