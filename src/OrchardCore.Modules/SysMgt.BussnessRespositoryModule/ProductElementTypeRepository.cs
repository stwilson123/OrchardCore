using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Repository;
using SysMgt.BussnessDTOModule.ProductElementType;
using BlocksCore.Data.EF.Linq;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule
{
   public class ProductElementTypeRepository:DBSqlRepositoryBase<BDTA_PRODUCTELEMENT_TYPE>, IProductElementTypeRepository
    {
        public ProductElementTypeRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
            
        }

       public PageList<ProductElementTypePageResult> GetPageList(ProductElementTypeSearchModel search)
       {
            var contextTable = GetContextTable();
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.IsVariable))
                {
                    contextTable = contextTable.Where((BDTA_PRODUCTELEMENT_TYPE eleType) => eleType.ISVARIABLE == search.IsVariable);
                }
            }
            return contextTable.Paging((BDTA_PRODUCTELEMENT_TYPE bdtaProductelementType)=>new ProductElementTypePageResult()
           {
               ID = bdtaProductelementType.Id,
               Code = bdtaProductelementType.CODE,
               Name = bdtaProductelementType.NAME,
               IsVariable = bdtaProductelementType.ISVARIABLE
           },search.page);
       }

        public PageList<ComboboxData> GetComboxList(ProductElementTypeSearchModel search)
        {
            return GetContextTable().Paging((BDTA_PRODUCTELEMENT_TYPE bdtaProductelementType) => new ComboboxData()
            {
                Id = bdtaProductelementType.Id,
                Text = bdtaProductelementType.NAME
            }, search.page);
        }

    }
}
