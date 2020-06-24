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
    public class ProductElementRepository:DBSqlRepositoryBase<BDTA_PRODUCTELEMENT>,IProductElementRepository
    {
        public ProductElementRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }
        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return GetContextTable().Paging((BDTA_PRODUCTELEMENT t) => new ComboboxData
            {
                Id = t.Id,
                Text = t.PRODUCTELEMENT_NAME
            }, search.page);
        }


        public PageList<ProductElementPageResult> GetPageList(ProductElementSearchModel productElementSearchModel)
        {
              return GetContextTable().Paging((BDTA_PRODUCTELEMENT b) => new ProductElementPageResult()
            {
                ID = b.Id,
                ProductElementCode = b.PRODUCTELEMENT_CODE,
                ProductElementName=b.PRODUCTELEMENT_NAME,
                ProductElementLength = b.PRODUCTELEMENT_LENGTH,
                ProductElementDescription = b.PRODUCTELEMENT_DESCRIPTION,
                ProductElementTypeName= b.BDTA_PRODUCTELEMENT_TYPE.NAME,
                  ResetDate=b.RESET_DATE,
                  AutoIncrement=b.AUTO_INCREMENT.ToString()
              }, productElementSearchModel.page);
        }
    }
}
