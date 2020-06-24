using BlocksCore.Application.Abstratctions.Datatransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.QuestionFeedBack
{
   public class QuestionFeedBackPageResult 
    {
        public string ID { get; set; }

        /// <summary>
        /// 反馈类型
        /// </summary>
        public string FeedbackType { get; set; }

        /// <summary>
        /// 反馈标题
        /// </summary>
        public string FeedbackTitle { get; set; }

        /// <summary>
        /// 反馈内容
        /// </summary>
        public string FeedbackContent { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creater { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string Updater { get; set; }
    }
}
