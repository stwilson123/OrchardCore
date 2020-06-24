using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.ProductElement;
using SysMgt.BussnessDomainModule.ProductElementType;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.ProductElement;
using SysMgt.BussnessDTOModule.ProductElementType;

namespace SysMgt.BussnessApplicationModule
{
    public class ProductElementAppService : AppService, IProductElementAppService
    {
        public ProductElementDomain productElementDomain { get; set; }

        public ProductElementAppService(ProductElementDomain productElementDomain)
        {
            this.productElementDomain = productElementDomain;
        }
        public string Add(ProductElementInfo productElementInfo)
        {
            ProductElementData productElementData=new ProductElementData();
            productElementData.ID = Guid.NewGuid().ToString();
            productElementData.Code = productElementInfo.Code;
            productElementData.Name = productElementInfo.Name;
            productElementData.ElementTypeId = productElementInfo.ElementTypeId;
            productElementData.Length = productElementInfo.Length;
            productElementData.Default = productElementInfo.Default;
            productElementData.Description = productElementInfo.Description;
            productElementData.AutoIncrement = productElementInfo.AutoIncrement;
            productElementData.ResetDate = productElementInfo.ResetDate;
            return productElementDomain.Add(productElementData);
        }

        public string Delete(ProductElementInfo productElementInfo)
        {
            ProductElementData productElementData = new ProductElementData();
            productElementData.IDS = productElementInfo.IDS;
            return productElementDomain.Delete(productElementData);
        }

        public string Update(ProductElementInfo productElementInfo)
        {
            ProductElementData productElementData = new ProductElementData();
            productElementData.ID = productElementInfo.ID;
            productElementData.Code = productElementInfo.Code;
            productElementData.Name = productElementInfo.Name;
            productElementData.ElementTypeId = productElementInfo.ElementTypeId;
            productElementData.Length = productElementInfo.Length;
            productElementData.Default = productElementInfo.Default;
            productElementData.Description = productElementInfo.Description;
            productElementData.AutoIncrement = productElementInfo.AutoIncrement;
            productElementData.ResetDate = productElementInfo.ResetDate;
            return  productElementDomain.Update(productElementData);
        }
        public PageList<ProductElementPageResult> GetPageList(ProductElementSearchModel search)
        {
            return productElementDomain.GetPageList(search);
        }

        public ProductElementInfo GetOneById(ProductElementInfo productElementInfo)
        {
            ProductElementData productElementData=new ProductElementData();
            productElementData.ID = productElementInfo.ID;
            productElementData = productElementDomain.GetOneById(productElementData);

            productElementInfo.Code = productElementData.Code;
            productElementInfo.Name = productElementData.Name;
            productElementInfo.ElementTypeId = productElementData.ElementTypeId;
            productElementInfo.Default = productElementData.Default;
            productElementInfo.Length = productElementData.Length;
            productElementInfo.Description = productElementData.Description;
            productElementInfo.AutoIncrement = productElementData.AutoIncrement;
            productElementInfo.ResetDate = productElementData.ResetDate;
            return productElementInfo;
        }

        public List<ProductElementInfo> GetAllList()
        {
            var productElementDatas = productElementDomain.GetAllList();
            List<ProductElementInfo> productElementInfos=new List<ProductElementInfo>();
            foreach (var item in productElementDatas)
            {
                productElementInfos.Add(new ProductElementInfo()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Length = item.Length
                });
            }

            return productElementInfos;
        }

        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return productElementDomain.GetComboxList(search);
        }
    }
}
