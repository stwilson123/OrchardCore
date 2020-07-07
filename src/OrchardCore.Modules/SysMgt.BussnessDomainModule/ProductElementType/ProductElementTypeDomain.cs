using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Domain.Abstractions.Domain;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.Localization;
using SysMgt.BussnessDTOModule.ProductElement;
using SysMgt.BussnessDTOModule.ProductElementType;
using SysMgt.BussnessRespositoryModule;

namespace SysMgt.BussnessDomainModule.ProductElementType
{
    public class ProductElementTypeDomain:IDomainService
    {
        public IProductElementTypeRepository ProductElementTypeRepository { get; set; }

        public IStringLocalizer L { get; set; }
        public ProductElementTypeDomain(IProductElementTypeRepository productElementTypeRepository)
        {
            this.ProductElementTypeRepository = productElementTypeRepository;
        }

        public virtual string Add(ProductElementTypeData productElementTypeData)
        {
            var productElementType=ProductElementTypeRepository.FirstOrDefault(t=>t.CODE==productElementTypeData.Code);
            if (productElementType != null)
            {
                throw new BlocksBussnessException("101", L["编码已存在!"], null);               
            }
            BDTA_PRODUCTELEMENT_TYPE bdtaProductelementType=new BDTA_PRODUCTELEMENT_TYPE();
            bdtaProductelementType.Id = Guid.NewGuid().ToString();
            if (productElementTypeData.Code == "")
            {
                throw new BlocksBussnessException("101", L["编码不能为空!"], null);
            }
            bdtaProductelementType.CODE = productElementTypeData.Code;
            if (productElementTypeData.Name == "")
            {
                throw new BlocksBussnessException("101", L["名称不能为空!"], null);
            }
            bdtaProductelementType.NAME = productElementTypeData.Name;
            //if (bdtaProductelementType.ISVARIABLE == null)
            //{
            //    throw new BlocksBussnessException("101", L["是否为变量必选!"], null);
            //}
            bdtaProductelementType.ISVARIABLE = productElementTypeData.IsVariable;
            var returnId = ProductElementTypeRepository.InsertAndGetId(bdtaProductelementType);
            if (string.IsNullOrEmpty(returnId))
            {
                //throw new BlocksBussnessException("101", L["保存失败!"], null);
                return "保存失败";
            }
            else
            {
                return "保存成功";
            }
        }

        public virtual string Delete(ProductElementTypeData productElementTypeData)
        {
            for (var i = 0; i < productElementTypeData.IDS.Count; i++)
            {
                var id = productElementTypeData.IDS[i];
                ProductElementTypeRepository.Delete(t => t.Id == id);
            }
          
            return "删除成功";
        }

        public virtual string Update(ProductElementTypeData productElementTypeData)
        {
            var productElementType = ProductElementTypeRepository.FirstOrDefault(t => t.CODE == productElementTypeData.Code);
            if (productElementType != null&& productElementTypeData.ID!= productElementType.Id)
            {

                throw new BlocksBussnessException("101", L["编码已存在"], null);
                
            }
            int successCount= ProductElementTypeRepository.Update(t => t.Id == productElementTypeData.ID,t=>new BDTA_PRODUCTELEMENT_TYPE()
            {
                CODE = productElementTypeData.Code,
                NAME = productElementTypeData.Name,
                ISVARIABLE = productElementTypeData.IsVariable
            });
            if (successCount > 0)
            {
                return "更新成功";
            }
            else
            {
                throw new BlocksBussnessException("101", L["编辑失败 ！"], null);
               
            }
        }

        public virtual ProductElementTypeData GetOneById(ProductElementTypeData productElementTypeData)
        {
            var productElementType= ProductElementTypeRepository.FirstOrDefault(t=>t.Id==productElementTypeData.ID);
            if (productElementType == null)
            {
                throw new BlocksBussnessException("101", L["未查到对象"], null);
            }
            return new ProductElementTypeData()
            {
                Code = productElementType.CODE,
                Name = productElementType.NAME,
                IsVariable = productElementType.ISVARIABLE
            };
        }

        public virtual PageList<ProductElementTypePageResult> GetPageList(ProductElementTypeSearchModel search)
        {
            return ProductElementTypeRepository.GetPageList(search);
        }

        public virtual PageList<ComboboxData> GetComboxList(ProductElementTypeSearchModel search)
        {
            return ProductElementTypeRepository.GetComboxList(search);
        }
    }
}
