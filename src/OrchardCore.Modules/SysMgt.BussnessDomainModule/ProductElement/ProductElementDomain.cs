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
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.ProductElement;
using SysMgt.BussnessDTOModule.ProductElementType;
using SysMgt.BussnessRespositoryModule;

namespace SysMgt.BussnessDomainModule.ProductElement
{
    public class ProductElementDomain : IDomainService
    {
        public IProductElementRepository ProductElementRepository { get; set; }
        public IProductFormatDetailRepository ProductFormatDetailRepository { get; set; }

        public IStringLocalizer L { get; set; }
        public ProductElementDomain(IProductElementRepository productElementRepository, IProductFormatDetailRepository productFormatDetailRepository)
        {
            this.ProductElementRepository = productElementRepository;
            this.ProductFormatDetailRepository = productFormatDetailRepository;
        }

        public virtual string Add(ProductElementData productElementData)
        {
            var productElement =
                ProductElementRepository.FirstOrDefault(t => t.PRODUCTELEMENT_CODE == productElementData.Code);
            if (productElement != null)
            {
                throw new BlocksBussnessException("101", L["编码已存在!"], null);
            }

            BDTA_PRODUCTELEMENT bdBdtaProductelement = new BDTA_PRODUCTELEMENT();
            bdBdtaProductelement.Id = Guid.NewGuid().ToString();
            if (productElementData.Code == "")
            {
                throw new BlocksBussnessException("101", L["编码不能为空!"], null);
            }
            bdBdtaProductelement.PRODUCTELEMENT_CODE = productElementData.Code;
            if (productElementData.Name == "")
            {
                throw new BlocksBussnessException("101", L["名称不能为空!"], null);
            }
            bdBdtaProductelement.PRODUCTELEMENT_NAME = productElementData.Name;
            bdBdtaProductelement.BDTA_PRODUCTELEMENT_TYPE_ID = productElementData.ElementTypeId;
            if (productElementData.ElementTypeId == "")
            {
                throw new BlocksBussnessException("101", L["请选择类型!"], null);
            }
            bdBdtaProductelement.PRODUCTELEMENT_LENGTH = productElementData.Length;
            bdBdtaProductelement.RESET_DATE = productElementData.ResetDate;//流水码规则
            if (productElementData.AutoIncrement == null)
            {
                bdBdtaProductelement.AUTO_INCREMENT = 1;
            }
            else
            {
                bdBdtaProductelement.AUTO_INCREMENT = int.Parse(productElementData.AutoIncrement);//自增量
            }
            if (productElementData.Length == "")
            {
                throw new BlocksBussnessException("101", L["位数不能为空!"], null);
            }
            bdBdtaProductelement.PRODUCTELEMENT_DESCRIPTION = productElementData.Description;
            bdBdtaProductelement.PRODUCTELEMENT_DEFAULT = productElementData.Default;
            string returntId = ProductElementRepository.InsertAndGetId(bdBdtaProductelement);
            if (string.IsNullOrEmpty(returntId))
            {
                return "保存失败！";
            }
            else
            {
                return "保存成功！";
            }
        }

        public virtual string Delete(ProductElementData productElementData)
        {
            for (var I = 0; I < productElementData.IDS.Count; I++)
            {
                var edate = ProductFormatDetailRepository.GetAllList(t => t.PRODUCTELEMENTID == productElementData.IDS[I]);
                if (edate.Count != 0)
                {
                    string name = ProductElementRepository.FirstOrDefault(t => t.Id == productElementData.IDS[I]).PRODUCTELEMENT_NAME;
                    throw new BlocksBussnessException("101", L[name+"已在编码规则中使用，不能删除!"], null);
                }
                ProductElementRepository.Delete(t => t.Id == productElementData.IDS[I]);
            }
            
            return "删除成功！";
        }

        public virtual string Update(ProductElementData productElementData)
        {
            var productElement = ProductElementRepository.FirstOrDefault(t => t.PRODUCTELEMENT_CODE == productElementData.Code);
            if (productElement != null && productElementData.ID != productElement.Id)
            {
                return "编码已存在";
            }
            var autoIncrement = productElementData.AutoIncrement;
            if (autoIncrement == null)
            {
                autoIncrement = "1";
            }
            int successCount = ProductElementRepository.Update(t => t.Id == productElementData.ID, t => new BDTA_PRODUCTELEMENT()
            {
                PRODUCTELEMENT_CODE = productElementData.Code,
                PRODUCTELEMENT_NAME = productElementData.Name,
                BDTA_PRODUCTELEMENT_TYPE_ID = productElementData.ElementTypeId,
                PRODUCTELEMENT_LENGTH = productElementData.Length,
                PRODUCTELEMENT_DESCRIPTION = productElementData.Description,
                PRODUCTELEMENT_DEFAULT = productElementData.Default,
                AUTO_INCREMENT = int.Parse(autoIncrement),
                RESET_DATE = productElementData.ResetDate
            });
            if (successCount > 0)
            {
                return "更新成功";
            }
            else
            {
                return "更新失败";
            }
        }

        public virtual PageList<ProductElementPageResult> GetPageList(ProductElementSearchModel search)
        {
            return ProductElementRepository.GetPageList(search);
        }

        public virtual ProductElementData GetOneById(ProductElementData productElementData)
        {
            var productElement = ProductElementRepository.FirstOrDefault(t => t.Id == productElementData.ID);
            if (productElement == null)
            {
                throw new BlocksBussnessException("101", L["未查到对象"], null);
            }

            return new ProductElementData()
            {
                ID = productElement.Id,
                Code = productElement.PRODUCTELEMENT_CODE,
                Name = productElement.PRODUCTELEMENT_NAME,
                ElementTypeId = productElement.BDTA_PRODUCTELEMENT_TYPE_ID,
                Length = productElement.PRODUCTELEMENT_LENGTH,
                Default = productElement.PRODUCTELEMENT_DEFAULT,
                Description = productElement.PRODUCTELEMENT_DESCRIPTION,
                ResetDate=productElement.RESET_DATE,
                AutoIncrement=productElement.AUTO_INCREMENT.ToString()

            };

        }

        public virtual List<ProductElementData> GetAllList()
        {
            var productelements = ProductElementRepository.GetAllList();
            List<ProductElementData> productElementDatas = new List<ProductElementData>();
            foreach (var item in productelements)
            {
                productElementDatas.Add(new ProductElementData()
                {
                    ID = item.Id,
                    Name = item.PRODUCTELEMENT_NAME,
                    Length = item.PRODUCTELEMENT_LENGTH
                });
            }

            return productElementDatas;
        }

        public virtual PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return ProductElementRepository.GetComboxList(search);
        }

    }
}