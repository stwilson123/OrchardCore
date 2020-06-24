using BlocksCore.Application.Abstratctions;
using SysMgt.BussnessDTOModule.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessApplicationModule
{
   public interface   ICommonAppService : IAppService
    {
        string GetImportConfigInfo(CommonEntity pInfo);
        string ImportData(CommonEntity pInfo);
        string UploadFile();
    }
}
