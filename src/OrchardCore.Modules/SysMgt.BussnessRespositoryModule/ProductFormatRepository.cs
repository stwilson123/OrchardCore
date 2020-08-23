using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.Abstractions.UnitOfWork;

using BlocksCore.Data.Linq;
using BlocksCore.Data.Linq2DB.Repository;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.ProductFormat;

namespace SysMgt.BussnessRespositoryModule
{
    public class ProductFormatRepository: DBSqlRepositoryBase<BDTA_PRODUCTFORMAT>, IProductFormatRepository
    {
        public ProductFormatRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        public PageList<ProductFormatPageResult> GetPageList(ProductFormatSearchModel search)
        {
            return GetContextTable()
                .Paging((BDTA_PRODUCTFORMAT bdtaProductformat) => new ProductFormatPageResult()
                {
                    ID = bdtaProductformat.Id,
                    Code = bdtaProductformat.PRODUCTFORMAT_CODE,
                    Name = bdtaProductformat.PRODUCTFORMAT_NAME,
                    Description = bdtaProductformat.PRODUCTFORMAT_DESCRIPTION,
                    Length = bdtaProductformat.PRODUCTFORMAT_LENGTH
                }, search.page);
        }

        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return GetContextTable().Paging((BDTA_PRODUCTFORMAT t) => new ComboboxData
            {
                Id = t.Id,
                Text = t.PRODUCTFORMAT_NAME
            }, search.page);
        }
    }
}
