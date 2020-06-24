using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Abstractions.Security;
using SysMgt.BussnessDomainModule.QuestionFeedBack;
using SysMgt.BussnessDTOModule.QuestionFeedBack;

namespace SysMgt.BussnessApplicationModule


{
    public class QuestionFeedBackAppService : AppService, IQuestionFeedBackAppService
    {

        private QuestionFeedBackDomain questionFeedBackDomain { get; set; }
        private IUserContext userContext { get; set; }

        public QuestionFeedBackAppService(QuestionFeedBackDomain questionFeedBackDomain, IUserContext userContext)
        {
            this.questionFeedBackDomain = questionFeedBackDomain;
            this.userContext = userContext;
        }
        public PageList<QuestionFeedBackPageResult> GetPageList(QuestionFeedBackSearchModel search)
        {
            return questionFeedBackDomain.GetPageList(search);
        }
        public string Add(QuestionFeedBackInfo model)
        { 
            QuestionFeedBackData data = new QuestionFeedBackData();            
            data.FeedbackTitle = model.FeedbackTitle;
            data.FeedbackType = model.FeedbackType;
            data.FeedbackContent = model.FeedbackContent;
            data.WebPath = model.WebPath;

            string userId = userContext.GetCurrentUser().UserId;//反馈人
            return questionFeedBackDomain.Add(data, userId);
        }

        public string AddNew(QuestionFeedBackInfo model)
        {
            QuestionFeedBackData data = new QuestionFeedBackData();
            data.FeedbackTitle = model.FeedbackTitle;
            data.FeedbackType = model.FeedbackType;
            data.FeedbackContent = model.FeedbackContent;

            string userId = userContext.GetCurrentUser().UserId;//反馈人
            return questionFeedBackDomain.AddNew(data, userId);
        }

        public QuestionFeedBackInfo GetOneById(QuestionFeedBackInfo  questionFeedBackInfo)
        {
            QuestionFeedBackData newData = new QuestionFeedBackData();
            newData.ID = questionFeedBackInfo.ID;
            newData.WebPath = questionFeedBackInfo.WebPath;
            newData = questionFeedBackDomain.GetOneById(newData);
            questionFeedBackInfo.FeedbackType = newData.FeedbackType;
            questionFeedBackInfo.FeedbackTitle = newData.FeedbackTitle;
            questionFeedBackInfo.FeedbackContent = newData.FeedbackContent;
            questionFeedBackInfo.WebPath = newData.WebPath;

            return questionFeedBackInfo;
        }

        public QuestionFeedBackInfo GetOneByIdNew(QuestionFeedBackInfo questionFeedBackInfo)
        {
            QuestionFeedBackData newData = new QuestionFeedBackData();
            newData.ID = questionFeedBackInfo.ID;
            newData.WebPath = questionFeedBackInfo.WebPath;
            newData = questionFeedBackDomain.GetOneByIdNew(newData);
            questionFeedBackInfo.FeedbackType = newData.FeedbackType;
            questionFeedBackInfo.FeedbackTitle = newData.FeedbackTitle;
            questionFeedBackInfo.FeedbackContent = newData.FeedbackContent;
            return questionFeedBackInfo;
        }
    }
}