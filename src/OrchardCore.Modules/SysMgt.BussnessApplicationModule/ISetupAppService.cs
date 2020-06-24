using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.Setup;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessApplicationModule
{
    public interface ISetupAppService : IAppService
    {
        PageList<SetupTypePageResult> GetSetupTypePageList(SetupTypeSearchModel search);
        string AddSetupTypeAndDetail(SetupTypeInfo setupTypeInfo);
        SetupTypeInfo GetSetupTypeById(SetupTypeInfo setupInfo);
        string EditSetupTypeAndDetail(SetupTypeInfo setupTypeInfo);
        string DeleteSetupTypeById(CommonEntity setupTypeId);

        PageList<SetupPageResult> GetPageList(SetupSearchModel search);
        string Add(SetupInfo setupInfo);
        string Update(SetupInfo outInTypeInfo);
        string Delete(CommonEntity entity4Delete);
        SetupData GetOneById(SetupInfo outInTypeInfo);
        SetupData GetOneByCode(SetupInfo setupInfo);
        PageList<ComboboxData> GetComboxListByPrinter(SearchModel search);
    }
}
