using Blocks.BussnessEntityModule;

using BlocksCore.Domain.Abstractions.Domain;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.Localization;
using SysMgt.BussnessRespositoryModule.QuestionFeedBack;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessRespositoryModule; 
using SysMgt.BussnessDTOModule.QuestionFeedBack; 
using System.IO;

namespace SysMgt.BussnessDomainModule.QuestionFeedBack
{
  public class QuestionFeedBackDomain : IDomainService
    {
        public IStringLocalizer L { get; set; }
        private IQuestionFeedBackRepository  questionFeedBackRepository { get; set; }

        public QuestionFeedBackDomain(IQuestionFeedBackRepository questionFeedBackRepository)
        {
            this.questionFeedBackRepository = questionFeedBackRepository;
        }

        public virtual PageList<QuestionFeedBackPageResult> GetPageList(QuestionFeedBackSearchModel search)
        {
            return questionFeedBackRepository.GetPageList(search);
        }

        public virtual QuestionFeedBackData GetOneById(QuestionFeedBackData  questionFeedBackData)
        {
            var questionFeedBackInfo = questionFeedBackRepository.FirstOrDefault(t => t.Id == questionFeedBackData.ID);
            if (questionFeedBackInfo == null)
            {
                throw new BlocksBussnessException("101", L["failed"], null);
            }
            questionFeedBackData.FeedbackType = questionFeedBackInfo.FEEDBACK_TYPE;
            questionFeedBackData.FeedbackTitle = questionFeedBackInfo.FEEDBACK_TITLE;
            var sb= Read(questionFeedBackData.WebPath + questionFeedBackData.ID + ".txt");
            questionFeedBackData.FeedbackContent = sb.ToString();//questionFeedBackInfo.FEEDBACK_CONTENT;
          
            return questionFeedBackData;
        }    

        public string Add(QuestionFeedBackData info,string userId)
        {
            #region 数据校验
            if (info == null)
            {
                throw new BlocksBussnessException("101", L["notnull", L["PARAMETER"]], null); 
            }
            if (string.IsNullOrEmpty(info.FeedbackType))
            {
                throw new BlocksBussnessException("101", L["notnull", L["FEEDBACK_TYPE"]], null);
            }
            if (string.IsNullOrEmpty(info.FeedbackTitle))
            {
                throw new BlocksBussnessException("101", L["notnull", L["FEEDBACK_TITLE"]], null);
            }
            if (info.FeedbackContent.Length==0)
            {
                throw new BlocksBussnessException("101", L["notnull", L["FEEDBACK_CONTENT"]], null);
            }
            //var feedBackData = questionFeedBackRepository.FirstOrDefault(t => t.FEEDBACK_TITLE==info.FeedbackTitle && t.CREATER== userId);
            //if (feedBackData != null)
            //{
            //    throw new BlocksBussnessException("101", L["您已经提过相同问题啦!"], null);
            //}
            #endregion

            #region 数据提交
            SYS_QUESTION_FEEDBACK data = new SYS_QUESTION_FEEDBACK();
            data.Id = Guid.NewGuid().ToString();
            data.FEEDBACK_TYPE = info.FeedbackType;
            data.FEEDBACK_TITLE = info.FeedbackTitle;
             
            Write(info.WebPath,data.Id,info.FeedbackContent);
            data.FEEDBACK_CONTENT = info.WebPath + data.Id + ".txt";  //内容中存的是文件应用程序物理路径            
            string addResultId=questionFeedBackRepository.InsertAndGetId(data);
            if (string.IsNullOrEmpty(addResultId))
            {
                throw new BlocksBussnessException("101", L["failed"], null);//反馈添加失败
            }
            #endregion

            return L["FEEDBACK_TIP_003"];//"感谢您提交宝贵建议！"
        }

        public QuestionFeedBackData GetOneByIdNew(QuestionFeedBackData questionFeedBackData)
        {
            var questionFeedBackInfo = questionFeedBackRepository.FirstOrDefault(t => t.Id == questionFeedBackData.ID);
            if (questionFeedBackInfo == null)
            {
                throw new BlocksBussnessException("101", L["failed"], null);
            }
            questionFeedBackData.FeedbackType = questionFeedBackInfo.FEEDBACK_TYPE;
            questionFeedBackData.FeedbackTitle = questionFeedBackInfo.FEEDBACK_TITLE;
            questionFeedBackData.FeedbackContent = questionFeedBackInfo.FEEDBACK_CONTENT;
            return questionFeedBackData;
        }

        public string AddNew(QuestionFeedBackData info, string userId)
        {
            #region 数据校验
            if (info == null)
            {
                throw new BlocksBussnessException("101", L["notnull", L["PARAMETER"]], null);
            }
            if (string.IsNullOrEmpty(info.FeedbackType))
            {
                throw new BlocksBussnessException("101", L["notnull", L["FEEDBACK_TYPE"]], null);
            }
            if (string.IsNullOrEmpty(info.FeedbackTitle))
            {
                throw new BlocksBussnessException("101", L["notnull", L["FEEDBACK_TITLE"]], null);
            }
            if (info.FeedbackContent.Length == 0)
            {
                throw new BlocksBussnessException("101", L["notnull", L["FEEDBACK_CONTENT"]], null);
            }
            #endregion

            #region 数据提交
            SYS_QUESTION_FEEDBACK data = new SYS_QUESTION_FEEDBACK();
            data.Id = Guid.NewGuid().ToString();
            data.FEEDBACK_TYPE = info.FeedbackType;
            data.FEEDBACK_TITLE = info.FeedbackTitle;
            data.FEEDBACK_CONTENT = info.FeedbackContent;         
            string addResultId = questionFeedBackRepository.InsertAndGetId(data);
            if (string.IsNullOrEmpty(addResultId))
            {
                throw new BlocksBussnessException("101", L["failed"], null);//反馈添加失败
            }
            #endregion

            return L["FEEDBACK_TIP_003"];//"感谢您提交宝贵建议！"
        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="path">文件物理路径（含文件名称）</param>
        /// <returns></returns>
        public StringBuilder Read(string path)
        {
            if (!File.Exists(path))
            {
                throw new BlocksBussnessException("101", L["FEEDBACK_TIP_004"], null);//反馈内容文件已缺失
            }
            string line;
            StringBuilder sb = new StringBuilder();             
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {                
                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line.ToString());
                }
            }
            return sb;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="webPath">应用程序物理路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="content">写入内容</param>
        public void Write(string webPath,string fileName,string content)
        {                
            if (!Directory.Exists(webPath))
            {
                Directory.CreateDirectory(webPath);
            }
            using (FileStream fs = new FileStream(webPath + fileName + ".txt", FileMode.Create)) 
            {  
                byte[] data = Encoding.Default.GetBytes(content);                
                fs.Write(data, 0, data.Length);               
                fs.Flush();
                fs.Close();
            }
        }

    }
}
