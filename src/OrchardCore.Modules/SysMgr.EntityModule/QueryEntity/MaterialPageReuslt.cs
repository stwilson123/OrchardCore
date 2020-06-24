using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Data.Abstractions.Entities;

namespace Blocks.BussnessEntityModule.QueryEntity
{
    public class MaterialPageReuslt : IQueryEntity
    {
        public string ID { get; set; }
        public string MaterialTypeName { get; set; }
        public string MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        //public decimal? Qty { get; set; }
        public string MaterialModel { get; set; }
        public string MaterialQuality { get; set; }
        public string Unit { get; set; }
        //public string UnitName { get; set; }
        public string UnitId { get; set; }
        public long? State { get; set; }
        public long? MinQty { get; set; }
        public string QaCheckModel { get; set; }
        //public string Desc { get; set; }
        public string MaterialDesc { get; set; }

        public string QaCheckModelOriginal { get; set; }
        public decimal? MaterialWeight { get; set; }
        /// <summary>
        /// 容积
        /// </summary>
        public decimal? MaterialVolume { get; set; }
        /// <summary>
        /// 长
        /// </summary>
        public decimal? MaterialLenght { get; set; }
        public decimal? MaterialWidth { get; set; }
        public decimal? MaterialHeight { get; set; }
        /// <summary>
        /// 启用容积/尺寸  1-启用容器   2-启用尺寸
        /// </summary>
        //public string OpenVolumeSize { get; set; }
        // public string ProductFormatID { get; set; }
    }

    public class MaterialPageReusltSQL : IQueryEntity
    {
        public string ID { get; set; }

        /// <summary>
        /// 物料类别
        /// </summary>
        public string MaterialTypeName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料规格
        /// </summary>
        public string MaterialModel { get; set; }

        /// <summary>
        /// 材质
        /// </summary>
        public string MaterialQuality { get; set; }


        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 是否判断物料
        /// </summary>

        public string RepeatedCheck { get; set; }

        /// <summary>
        /// 最小库存数
        /// </summary>

        public decimal? MinQty { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal? MaterialWeight { get; set; }

        /// <summary>
        /// 容积
        /// </summary>

        public decimal? MaterialVolume { get; set; }

        /// <summary>
        /// 长
        /// </summary>


        public long? MaterialLenght { get; set; }

        /// <summary>
        /// 宽
        /// </summary>

        public decimal? MaterialWidth { get; set; }

        /// <summary>
        /// 高
        /// </summary>

        public decimal? MaterialHeight { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string MaterialDesc { get; set; }


    }
    public class MaterialReuslt : IQueryEntity
    {
        public string ID { get; set; }
        public string STOREROM_ID { get; set; }
        public string MaterialTypeName { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string UnitName { get; set; }
        public string MaterialModel { get; set; }
        public string MaterialQuality { get; set; }
        public long? State { get; set; }
        public long? MinQty { get; set; }
        public string QaCheckModelOriginal { get; set; }

    }
    public class MaterialStockReuslt : IQueryEntity
    {
        public string ID { get; set; }
        public string UnitName { get; set; }
        public string MaterialTypeName { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string MaterialModel { get; set; }
        public string MaterialQuality { get; set; }
        public long? State { get; set; }
        public long? MinQty { get; set; }
        public string QaCheckModelOriginal { get; set; }
        public decimal? Qty { get; set; }

    }

    public class DeleteReuslt : IQueryEntity
    {
        public string ID { get; set; }
        public string Code { get; set; }

        public string Tip { get; set; }

    }
    public class DeleteIReuslt : IQueryEntity
    {


    }
}
