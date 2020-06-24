using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.Department;
using SysMgt.BussnessDTOModule;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.QuestionFeedBack;

namespace SysMgt.BussnessApplicationModule
{
    public interface IQuestionFeedBackAppService : IAppService
    {
        PageList<QuestionFeedBackPageResult> GetPageList(QuestionFeedBackSearchModel search);
        string Add(QuestionFeedBackInfo model);
        string AddNew(QuestionFeedBackInfo model);
        QuestionFeedBackInfo GetOneById(QuestionFeedBackInfo questionFeedBackInfo);
        QuestionFeedBackInfo GetOneByIdNew(QuestionFeedBackInfo questionFeedBackInfo);
    }
}
