using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.ConfigFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessRespositoryModule.ConfigFiles
{
    public interface IConfigFilesRepository : IRepository<BDTA_CONFIGFILES>
    {
        PageList<ConfigFilesPageResult> GetPageList(ConfigFilesSearchModel search);

    }
}
