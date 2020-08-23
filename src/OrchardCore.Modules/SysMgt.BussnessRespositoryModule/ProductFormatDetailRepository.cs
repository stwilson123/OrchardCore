using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.Abstractions.UnitOfWork;

using BlocksCore.Data.Linq;
using BlocksCore.Data.Linq2DB.Repository;
using SysMgt.BussnessDTOModule.ProductFormatDetail;

namespace SysMgt.BussnessRespositoryModule
{
   public class ProductFormatDetailRepository:DBSqlRepositoryBase<BDTA_PRODUCTFORMAT_DETAIL>, IProductFormatDetailRepository
    {
        public ProductFormatDetailRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
            
        }


        public List<ProductFormatDetailInfo> GetListByFormatId(string formatId)
        {
            return GetContextTable().Where((BDTA_PRODUCTFORMAT_DETAIL bdtaProductformatDetail) => bdtaProductformatDetail.PRODUCTFORMATID == formatId).SelectToList((BDTA_PRODUCTFORMAT_DETAIL bdtaProductformatDetail) => new ProductFormatDetailInfo()
            {
                ID = bdtaProductformatDetail.Id,
                Name = bdtaProductformatDetail.PRODUCTFORMAT_DETAIL_NAME,
                Length = bdtaProductformatDetail.PRODUCTFORMAT_DETAIL_LENTH,
                ProductElementTypeID=bdtaProductformatDetail.BDTA_PRODUCTELEMENT.BDTA_PRODUCTELEMENT_TYPE_ID,
                ProductElementType = bdtaProductformatDetail.BDTA_PRODUCTELEMENT.BDTA_PRODUCTELEMENT_TYPE.CODE,
                ProductElementTypeIsVariable = bdtaProductformatDetail.BDTA_PRODUCTELEMENT.BDTA_PRODUCTELEMENT_TYPE.ISVARIABLE,
                Seq = bdtaProductformatDetail.PRODUCTFORMAT_DETAIL_SEQ,
                Default = bdtaProductformatDetail.BDTA_PRODUCTELEMENT.PRODUCTELEMENT_DEFAULT,
                ProductElementId = bdtaProductformatDetail.PRODUCTELEMENTID,
                Start = bdtaProductformatDetail.PRODUCTFORMAT_START,
                End = bdtaProductformatDetail.PRODUCTFORMAT_END
            });

            ////return GetContextTable().SelectToList((BDTA_PRODUCTFORMAT_DETAIL bdtaProductformatDetail) => new BDTA_PRODUCTFORMAT_DETAIL()
            ////{
            ////    Id = bdtaProductformatDetail.Id,
            ////    PRODUCTFORMAT_DETAIL_NAME = bdtaProductformatDetail.PRODUCTFORMAT_DETAIL_NAME,
            ////    PRODUCTFORMAT_DETAIL_LENTH = bdtaProductformatDetail.PRODUCTFORMAT_DETAIL_LENTH
            ////});
            //return null;
        }

        public PageList<ProductFormatDetailPageResult> GetPageList(ProductFormatDetailSearchModel search)
        {
            var productformatDetails = GetContextTable();
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.ProductFormatID))
                {
                    productformatDetails = productformatDetails.Where((BDTA_PRODUCTFORMAT_DETAIL bdtaProductformatDetail) => bdtaProductformatDetail.PRODUCTFORMATID == search.ProductFormatID);
                }
            }


            return productformatDetails.OrderBy(t => t.PRODUCTFORMAT_DETAIL_SEQ).Paging((BDTA_PRODUCTFORMAT_DETAIL bdtaProductformatDetail) => new ProductFormatDetailPageResult()
                {
                    ID = bdtaProductformatDetail.Id,
                    Name = bdtaProductformatDetail.PRODUCTFORMAT_DETAIL_NAME,
                    Length = bdtaProductformatDetail.PRODUCTFORMAT_DETAIL_LENTH,
                    Seq = bdtaProductformatDetail.PRODUCTFORMAT_DETAIL_SEQ,
                ProductformatStart= bdtaProductformatDetail.PRODUCTFORMAT_START,
                ProductformatEnd= bdtaProductformatDetail.PRODUCTFORMAT_END
            }, search.page);
        }
    }
}
