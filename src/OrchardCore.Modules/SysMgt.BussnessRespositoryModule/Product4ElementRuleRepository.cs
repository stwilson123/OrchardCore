using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Repository;
using BlocksCore.Data.EF.Linq;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using SysMgt.BussnessDTOModule.ProductElement;
using SysMgt.BussnessDTOModule.ProductElementType;
using SysMgt.BussnessDTOModule.Combobox;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule
{
    public class Product4ElementRuleRepository: DBSqlRepositoryBase<BDTA_PRODUCT_ELEMENT_RULE_REL>,IProduct4ElementRuleRepository
    {
        public Product4ElementRuleRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

    }
}
