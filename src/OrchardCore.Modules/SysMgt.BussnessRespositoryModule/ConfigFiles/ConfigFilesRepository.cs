using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;

using BlocksCore.Data.Linq2DB.Repository;
using BlocksCore.Data.Linq;
using System.Collections.Generic;
using SysMgt.BussnessDTOModule.ConfigFiles;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule.ConfigFiles
{

    public class ConfigFilesRepository : DBSqlRepositoryBase<BDTA_CONFIGFILES>, IConfigFilesRepository
    {
        public ConfigFilesRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        public PageList<ConfigFilesPageResult> GetPageList(ConfigFilesSearchModel search)
        {
            return GetContextTable()
                .InnerJoin((WAREHOUSE_STOCK_LOG t) => t.CREATER, (SYS_USERINFO s) => s.Id)
                .Where((BDTA_CONFIGFILES t) => t.ISDELETE == 0)
                .Paging((BDTA_CONFIGFILES t, SYS_USERINFO s) => new ConfigFilesPageResult()
            {
                Id = t.Id,
                FileType = t.FILE_TYPE,
                FileName = t.FILE_NAME,
                FilePath = t.FILE_PATH,
                FileFunction = t.FILE_FUNCTION,
                CreateDate = t.CREATEDATE,
                Creater = s.CNAME
                }, search.page);
        }
    }
}
