using BlocksCore.Application.Abstratctions.Datatransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.Product4ElementInfo
{
   public class ProductVarElementInfo 
    {
        /// <summary>
        /// 业务功能ID
        /// </summary>
        public string ProductFuncID { get; set; }

        /// <summary>
        /// 编码元素ID集合
        /// </summary>
        public List<string> ProductElementTypeIDs { get; set; }
    }

    public class ProductElementRuleInfo 
    {
        /// <summary>
        /// 业务功能ID
        /// </summary>
        public string ProductFuncID { get; set; }

        /// <summary>
        /// 编码元素ID集合
        /// </summary>
        public List<string> ProductElementRuleIDs { get; set; }
    }
}
