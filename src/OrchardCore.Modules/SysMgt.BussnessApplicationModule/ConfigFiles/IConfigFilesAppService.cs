using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.ConfigFiles;
using System.Collections.Generic;
using System.Web;

namespace SysMgt.BussnessApplicationModule.ConfigFiles
{
    public interface IConfigFilesAppService : IAppService
    {
        PageList<ConfigFilesPageResult> GetPageList(ConfigFilesSearchModel search);
        ConfigFilesInfo GetOneById(ConfigFilesInfo info);
        string Add(ConfigFilesInfo info);
        string Update(ConfigFilesInfo info);
        string Delete(CommonEntity info);
        
        //string UploadFile();
        void downfile(ConfigFilesInfo configFilesInfo);
    }
}
