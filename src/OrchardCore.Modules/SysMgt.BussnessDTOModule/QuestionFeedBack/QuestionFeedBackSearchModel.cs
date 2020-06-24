using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Application.Abstratctions.Datatransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.QuestionFeedBack
{
   public class QuestionFeedBackSearchModel 
    {
        [DataTransfer("page")]
        public Page page { get; set; }
    }
}
