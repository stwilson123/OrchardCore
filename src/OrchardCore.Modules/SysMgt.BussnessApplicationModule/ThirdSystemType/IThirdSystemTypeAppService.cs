using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging; 
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.ThirdSystemType;
using System.Collections.Generic;

namespace SysMgt.BussnessApplicationModule.ThirdSystemCall
{
    public interface IThirdSystemTypeAppService : IAppService
    {
        PageList<ThirdSystemTypePageResult> GetPageList(ThirdSystemTypeSearchModel search);
        ThirdSystemTypeInfo GetOneById(ThirdSystemTypeInfo info);
        string Add(ThirdSystemTypeInfo info);
        string Update(ThirdSystemTypeInfo info);
        string Delete(CommonEntity info);
        List<ComboboxData> GetComboxList(); 
    }
}
