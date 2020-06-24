using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.Api
{
    public class RtnData
    {
        /// <summary>
        /// 返回值 1- 成功 0-失败
        /// </summary>
        public string RtnCode { get; set; }
        /// <summary>
        /// 返回说明
        /// </summary>
        public string RtnNote { get; set; }

    }
}
