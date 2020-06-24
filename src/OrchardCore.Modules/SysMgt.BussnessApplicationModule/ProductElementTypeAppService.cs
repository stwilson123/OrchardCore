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
   public class ProductElementTypeAppService:AppService, IProductElementTypeAppService
    {
        public ProductElementTypeDomain productElementTypeDomain { get; set; }

        public ProductElementTypeAppService(ProductElementTypeDomain productElementTypeDomain)
        {
            this.productElementTypeDomain = productElementTypeDomain;
        }

        public string Add(ProductElementTypeInfo productElementTypeInfo)
        {
            ProductElementTypeData productElementTypeData=new ProductElementTypeData();
            productElementTypeData.Code = productElementTypeInfo.Code;
            productElementTypeData.Name = productElementTypeInfo.Name;
            productElementTypeData.IsVariable = productElementTypeInfo.IsVariable;
            return productElementTypeDomain.Add(productElementTypeData);
        }

        public string Delete(ProductElementTypeInfo productElementTypeInfo)
        {
            ProductElementTypeData productElementTypeData=new ProductElementTypeData();
            productElementTypeData.IDS = productElementTypeInfo.IDS;
            return productElementTypeDomain.Delete(productElementTypeData);
        }

        public string Update(ProductElementTypeInfo productElementTypeInfo)
        {
            ProductElementTypeData productElementTypeData=new ProductElementTypeData();
            productElementTypeData.ID = productElementTypeInfo.ID;
            productElementTypeData.Code = productElementTypeInfo.Code;
            productElementTypeData.Name = productElementTypeInfo.Name;
            productElementTypeData.IsVariable = productElementTypeInfo.IsVariable;
            return productElementTypeDomain.Update(productElementTypeData);
        }

        public ProductElementTypeInfo GetOneById(ProductElementTypeInfo productElementTypeInfo)
        {
            ProductElementTypeData productElementTypeData=new ProductElementTypeData();
            productElementTypeData.ID = productElementTypeInfo.ID;
            productElementTypeData = productElementTypeDomain.GetOneById(productElementTypeData);

            productElementTypeInfo.Code = productElementTypeData.Code;
            productElementTypeInfo.Name = productElementTypeData.Name;
            productElementTypeInfo.IsVariable = productElementTypeData.IsVariable;
            return productElementTypeInfo;

        }

        public PageList<ProductElementTypePageResult> GetPageList(ProductElementTypeSearchModel search)
        {
            return productElementTypeDomain.GetPageList(search);
        }

        public PageList<ComboboxData> GetComboxList(ProductElementTypeSearchModel search)
        {
            return productElementTypeDomain.GetComboxList(search);
        }
    }
}
