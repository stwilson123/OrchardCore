using BlocksCore.Application.Abstratctions;
using SysMgt.BussnessDomainModule.Product4ElementInfo;
using SysMgt.BussnessDTOModule.Product4ElementInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessApplicationModule
{
  public class Product4ElementInfoAppService: AppService, IProduct4ElementInfoAppService
    {
        private Product4ElementInfoDomain Product4ElementInfoDomain { get; set; }

        public Product4ElementInfoAppService(Product4ElementInfoDomain product4ElementInfoDomain)
        {
            this.Product4ElementInfoDomain = product4ElementInfoDomain;
        }
         public string SaveProductVarElement(ProductVarElementInfo product4VarElementInfo)       
        {           
            return Product4ElementInfoDomain.SaveProductVarElement(product4VarElementInfo);
        }

        public ProductVarElementInfo GetValElementListByProFuncID(ProductVarElementInfo pro4VarElementInfo)
        {
            return Product4ElementInfoDomain.GetValElementListByProFuncID(pro4VarElementInfo);
        }

        public string SaveProductElementRule(ProductElementRuleInfo product4ElementRuleInfo)
        {
            return Product4ElementInfoDomain.SaveProductElementRule(product4ElementRuleInfo);
        }

        public ProductElementRuleInfo GetElementRuleListByProFuncID(ProductElementRuleInfo product4ElementRuleInfo)
        {
            return Product4ElementInfoDomain.GetElementRuleListByProFuncID(product4ElementRuleInfo);
        }
    }
}
