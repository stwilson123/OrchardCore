using BlocksCore.Abstractions.Datatransfer;
using System;
 
namespace SysMgt.BussnessDTOModule.ThirdSystemCall
{
   public class ThirdSystemCallInfo 
    {
        public string ID { get; set; }

        /// <summary>
        /// 第三方系统ID （系统类型表PK）
        /// </summary>
        public string SystemID { get; set; }

        /// <summary>
        /// 第三方系统编号
        /// </summary>
        public string SystemNo { get; set; }

        /// <summary>
        /// 第三方系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// 传入参数
        /// </summary>
        public string ParameterIn { get; set; }

        /// <summary>
        /// 返回信息（包括正常和异常返回）
        /// </summary>
        public string ResponseValue { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMsg { get; set; }

        /// <summary>
        /// 开始处理时间
        /// </summary>
        public DateTime? ProcessTimeBegin { get; set; }

        /// <summary>
        /// 结束处理时间
        /// </summary>
        public DateTime? ProcessTimeEnd { get; set; }

        /// <summary>
        /// 请求次数
        /// </summary>
        public long RequestTimes { get; set; }

        /// <summary>
        /// 处理结果  0-未有结果；1-成功；2-失败
        /// </summary>
        public long ProcessResult { get; set; }

    }

    public class ThirdSystemCallLog 
    {

        public string ID { get; set; }

        /// <summary>
        /// 第三方系统ID （系统类型表PK）
        /// </summary>
        public string SystemID { get; set; }

        /// <summary>
        /// 第三方系统编号
        /// </summary>
        public string SystemNo { get; set; }

        /// <summary>
        /// 第三方系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// 传入参数
        /// </summary>
        public string ParameterIn { get; set; }

        /// <summary>
        /// 返回信息（包括正常和异常返回）
        /// </summary>
        public string ResponseValue { get; set; }
    }
}
