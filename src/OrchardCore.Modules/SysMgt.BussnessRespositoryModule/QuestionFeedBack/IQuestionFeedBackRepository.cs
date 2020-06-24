using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.QuestionFeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessRespositoryModule.QuestionFeedBack
{
   public interface IQuestionFeedBackRepository : IRepository<SYS_QUESTION_FEEDBACK>
    {
        PageList<QuestionFeedBackPageResult> GetPageList(QuestionFeedBackSearchModel search);
    }
}
