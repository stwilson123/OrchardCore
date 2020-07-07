using BlocksCore.Domain.Abstractions.Domain;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.Localization;
using SysMgt.BussnessDTOModule;
using SysMgt.BussnessDTOModule.Product4ElementInfo;
using SysMgt.BussnessRespositoryModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;

namespace SysMgt.BussnessDomainModule.Product4ElementInfo
{
    public class Product4ElementInfoDomain : IDomainService
    {
        public IProduct4VarElementRepository Product4VarElementRepository { get; set; }

        public IProduct4ElementRuleRepository Product4ElementRuleRepository { get; set; }

        public IProductFormatDetailRepository ProductFormatDetailRepository { get; set; }

        public IStringLocalizer L { get; set; }

        public Product4ElementInfoDomain(IProduct4VarElementRepository product4VarElementRepository,
            IProduct4ElementRuleRepository product4ElementRuleRepository,
            IProductFormatDetailRepository productFormatDetailRepository
            )
        {
            this.Product4VarElementRepository = product4VarElementRepository;
            this.Product4ElementRuleRepository = product4ElementRuleRepository;
            this.ProductFormatDetailRepository = productFormatDetailRepository;
        }

        public virtual string SaveProductVarElement(ProductVarElementInfo pro4VarElementInfo)
        {
            if (pro4VarElementInfo == null)
            {
                throw new BlocksBussnessException("101", L["传入参数有误!"], null);
            }
            var varElementTypeIds = pro4VarElementInfo.ProductElementTypeIDs;
            //if (varElementTypeIds==null || varElementTypeIds.Count <= 0)
            //{
            //    throw new BlocksBussnessException("101", L["没有要保存的数据!"], null);
            //}
            List<BDTA_PRODUCT_VARELEMENT_REL> newRels = new List<BDTA_PRODUCT_VARELEMENT_REL>();
            foreach (var typeId in varElementTypeIds)
            {
                BDTA_PRODUCT_VARELEMENT_REL rel = new BDTA_PRODUCT_VARELEMENT_REL();
                rel.Id = Guid.NewGuid().ToString();
                rel.PRODUCT_FUNC_ID = pro4VarElementInfo.ProductFuncID;
                rel.PRODUCT_ELEMENT_TYPE_ID = typeId;
                newRels.Add(rel);
            }
            Product4VarElementRepository.Delete(t => t.PRODUCT_FUNC_ID == pro4VarElementInfo.ProductFuncID);
            if (newRels.Count > 0)
            {
                var result = Product4VarElementRepository.Insert(newRels);
                if (result.Count() <= 0)
                {
                    throw new BlocksBussnessException("101", L["保存失败!"], null);
                }
            }
            return "保存成功";
        }

        public virtual ProductVarElementInfo GetValElementListByProFuncID(ProductVarElementInfo pro4VarElementInfo)
        {
            if (pro4VarElementInfo == null || string.IsNullOrEmpty(pro4VarElementInfo.ProductFuncID))
            {
                throw new BlocksBussnessException("101", L["传入参数有误!"], null);
            }
            ProductVarElementInfo rtnData = new ProductVarElementInfo();
            rtnData.ProductFuncID = pro4VarElementInfo.ProductFuncID;
            var list = Product4VarElementRepository.GetAllList(x => x.PRODUCT_FUNC_ID == pro4VarElementInfo.ProductFuncID).ToList();
            List<string> productElementTypeIDs = new List<string>();
            foreach (var item in list)
            {
                productElementTypeIDs.Add(item.PRODUCT_ELEMENT_TYPE_ID);
            }
            rtnData.ProductElementTypeIDs = productElementTypeIDs;
            return rtnData;

        }

        public virtual string SaveProductElementRule(ProductElementRuleInfo pro4ElementRuleInfo)
        {
            if (pro4ElementRuleInfo == null)
            {
                throw new BlocksBussnessException("101", L["传入参数有误!"], null);
            }
            var elementRuleIds = pro4ElementRuleInfo.ProductElementRuleIDs;
            //if (elementRuleIds == null || elementRuleIds.Count <= 0)
            //{
            //    throw new BlocksBussnessException("101", L["没有要保存的编码规则数据!"], null);
            //}
            if (elementRuleIds.Count > 1)
            {
                throw new BlocksBussnessException("101", L["只能选择一条编码规则数据保存!"], null);
            }

            //获得此业务功能支持的外部元素集合
            BDTA_PRODUCT_ELEMENT_RULE_REL rel = new BDTA_PRODUCT_ELEMENT_RULE_REL();
            if (elementRuleIds.Count == 1)
            {
                var ruleId = elementRuleIds[0];
                rel.Id = Guid.NewGuid().ToString();
                rel.PRODUCT_FUNC_ID = pro4ElementRuleInfo.ProductFuncID;
                rel.PRODUCT_ELEMENT_RULE_ID = ruleId;

                //获得此业务功能支持的外部元素集合
                var proVarElementList = Product4VarElementRepository.GetAllList(x => x.PRODUCT_FUNC_ID == pro4ElementRuleInfo.ProductFuncID).ToList();

                #region  校验此业务功能点是否可以支持下面规则中的外部元素

                //编码规则的编码元素集合
                var ruleList = ProductFormatDetailRepository.GetListByFormatId(ruleId);
                if (ruleList == null || ruleList.Count == 0)
                {
                    throw new BlocksBussnessException("101", L["所选择的编码规则没有找到规则明细数据!"], null);
                }
                //编码规则的外部元素类型集合
                var elementTypeIDList = ruleList.Where(x => x.ProductElementTypeIsVariable == "0");
                foreach (var item in elementTypeIDList)
                {
                    var elementType = proVarElementList.Select(x => x.PRODUCT_ELEMENT_TYPE_ID).Contains(item.ProductElementTypeID);
                    if (!elementType)
                    {
                        throw new BlocksBussnessException("101", L["该业务功能不支持编码规则中元素名称为【{0}】所对应的编码类型!", item.Name], null);
                    }
                }
                #endregion
            }

            Product4ElementRuleRepository.Delete(t => t.PRODUCT_FUNC_ID == pro4ElementRuleInfo.ProductFuncID);
            if (rel.PRODUCT_FUNC_ID != null)
            {
                var result = Product4ElementRuleRepository.InsertAndGetId(rel);
                if (string.IsNullOrEmpty(result))
                {
                    throw new BlocksBussnessException("101", L["保存失败!"], null);
                }
            }
            return "保存成功";
        }

        public virtual ProductElementRuleInfo GetElementRuleListByProFuncID(ProductElementRuleInfo pro4ElementRuleInfo)
        {
            if (pro4ElementRuleInfo == null || string.IsNullOrEmpty(pro4ElementRuleInfo.ProductFuncID))
            {
                throw new BlocksBussnessException("101", L["传入参数有误!"], null);
            }
            ProductElementRuleInfo rtnData = new ProductElementRuleInfo();
            rtnData.ProductFuncID = pro4ElementRuleInfo.ProductFuncID;
            var list = Product4ElementRuleRepository.GetAllList(x => x.PRODUCT_FUNC_ID == pro4ElementRuleInfo.ProductFuncID).ToList();
            List<string> productElementRuleIDs = new List<string>();
            foreach (var item in list)
            {
                productElementRuleIDs.Add(item.PRODUCT_ELEMENT_RULE_ID);
            }
            rtnData.ProductElementRuleIDs = productElementRuleIDs;
            return rtnData;

        }

    }
}
