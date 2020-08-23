using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;

using BlocksCore.Data.Linq2DB.Repository;
using SysMgt.BussnessDTOModule.QuestionFeedBack;
using BlocksCore.Data.Linq;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule.QuestionFeedBack
{
  public  class QuestionFeedBackRepository : DBSqlRepositoryBase<SYS_QUESTION_FEEDBACK>, IQuestionFeedBackRepository
    {
        public QuestionFeedBackRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        public PageList<QuestionFeedBackPageResult> GetPageList(QuestionFeedBackSearchModel search)
        {
            return GetContextTable()
                .InnerJoin((SYS_QUESTION_FEEDBACK t)=>t.CREATER,(SYS_USERINFO s)=>s.Id)
                .Paging((SYS_QUESTION_FEEDBACK t, SYS_USERINFO s) => new QuestionFeedBackPageResult()
            {
                ID = t.Id,
                FeedbackType = t.FEEDBACK_TYPE,
                FeedbackTitle = t.FEEDBACK_TITLE,
                FeedbackContent = t.FEEDBACK_CONTENT,
                CreateDate = t.CREATEDATE,
                Creater = s.CNAME
            }, search.page);
        }
    }
}
