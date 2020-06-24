using BlocksCore.Application.Abstratctions;
using SysMgt.BussnessDTOModule.Product4ElementInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessApplicationModule
{
   public interface IProduct4ElementInfoAppService: IAppService
    {
        string SaveProductVarElement(ProductVarElementInfo product4VarElementInfo);

        ProductVarElementInfo GetValElementListByProFuncID(ProductVarElementInfo pro4VarElementInfo);

        string SaveProductElementRule(ProductElementRuleInfo product4VarElementInfo);

        ProductElementRuleInfo GetElementRuleListByProFuncID(ProductElementRuleInfo pro4ElementRuleInfo);
    }
}
