using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.ProductFormat;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.ProductElementType;
using SysMgt.BussnessDTOModule.ProductFormat;

namespace SysMgt.BussnessApplicationModule
{
    public class ProductFormatAppService : AppService, IProductFormatAppService
    {
        private ProductFormatDomain productFormatDomain { get; set; }
        public ProductFormatAppService(ProductFormatDomain productFormatDomain)
        {
            this.productFormatDomain = productFormatDomain;
        }
        public string Add(ProductFormatInfo productFormatInfo)
        {
            ProductFormatData productFormatData=new ProductFormatData();
            productFormatData.ID = Guid.NewGuid().ToString();
            productFormatData.Code = productFormatInfo.Code;
            productFormatData.Name = productFormatInfo.Name;
            productFormatData.Description = productFormatInfo.Description;

            List<BussnessDomainModule.ProductFormat.ProductElementInfo> list=new List<BussnessDomainModule.ProductFormat.ProductElementInfo>();
            foreach (var item in productFormatInfo.ProductElements)
            {
                list.Add(new BussnessDomainModule.ProductFormat.ProductElementInfo()
                {
                    ID= Guid.NewGuid().ToString(),
                    ProductElementID = item.ProductElementID,
                    ProductElementLength = item.ProductElementLength,
                    ProductElementName = item.ProductElementName,
                    ProductformatStart = item.ProductformatStart,
                    ProductformatEnd=item.ProductformatEnd
                }); 
            }
            productFormatData.ProductElements = list;

            return productFormatDomain.Add(productFormatData);
        }

        public string Delete(ProductFormatInfo productFormatInfo)
        {
            //throw new NotImplementedException();
            ProductFormatData productFormatData = new ProductFormatData();
            productFormatData.IDS = productFormatInfo.IDS;
            return productFormatDomain.Delete(productFormatData);
        }

        public PageList<ProductFormatPageResult> GetPageList(ProductFormatSearchModel search)
        {
            return productFormatDomain.GetPageList(search);
        }

        public string Update(ProductFormatInfo productFormatInfo)
        {
            ProductFormatData productFormatData = new ProductFormatData();
            productFormatData.ID = productFormatInfo.ID;
            productFormatData.Code = productFormatInfo.Code;
            productFormatData.Name = productFormatInfo.Name;
            productFormatData.Description = productFormatInfo.Description;

            List<BussnessDomainModule.ProductFormat.ProductElementInfo> list = new List<BussnessDomainModule.ProductFormat.ProductElementInfo>();
            foreach (var item in productFormatInfo.ProductElements)
            {
                list.Add(new BussnessDomainModule.ProductFormat.ProductElementInfo()
                {
                    ProductElementID = item.ProductElementID,
                    ProductElementLength = item.ProductElementLength,
                    ProductElementName = item.ProductElementName,
                    ProductformatStart = item.ProductformatStart,
                    ProductformatEnd = item.ProductformatEnd
                });
            }
            productFormatData.ProductElements = list;

            return productFormatDomain.Update(productFormatData);
        }

        public ProductFormatInfo GetOneById(ProductFormatInfo productFormatInfo)
        {
            ProductFormatData productFormatData=new ProductFormatData();
            productFormatData.ID = productFormatInfo.ID;
            productFormatData = productFormatDomain.GetOneById(productFormatData);
            productFormatInfo.Code = productFormatData.Code;
            productFormatInfo.Name = productFormatData.Name;
            productFormatInfo.Description = productFormatData.Description;

            List<BussnessDTOModule.ProductFormat.ProductElementInfo> list = new List<BussnessDTOModule.ProductFormat.ProductElementInfo>();

            foreach (var item in productFormatData.ProductElements)
            {
                list.Add(new BussnessDTOModule.ProductFormat.ProductElementInfo()
                {
                    ID = item.ID,
                    ProductElementID = item.ProductElementID,
                    ProductElementName=item.ProductElementName,
                    ProductElementLength = item.ProductElementLength,
                    ProductformatStart=item.ProductformatStart,
                    ProductformatEnd=item.ProductformatEnd
                });
            }

            productFormatInfo.ProductElements = list;


            return productFormatInfo;
        }

        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return productFormatDomain.GetComboxList(search);
        }

        public List<string> GetNumbers(CodeElementInfo codeElementInfo)
        {
            return productFormatDomain.GetNumbers(codeElementInfo.Dic, codeElementInfo.Code, codeElementInfo.Number);
        }

    }
}
