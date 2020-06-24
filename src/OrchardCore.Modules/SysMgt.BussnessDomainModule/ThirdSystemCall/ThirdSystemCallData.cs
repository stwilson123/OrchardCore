using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.ThirdSystemCall
{
   
    public class ThirdSystemCallData  
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
        public int RequestTimes { get; set; }

        /// <summary>
        /// 处理结果  0-新建；1-请求中；2-成功；3-失败（调用成功单结果失败和调用失败）
        /// </summary>
        public int ProcessResult { get; set; }

    }

    public class ThirdSystemCallLogData  
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
