using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Repository;
using SysMgt.BussnessDTOModule.Setup;
using BlocksCore.Data.EF.Linq;
using BlocksCore.Abstractions.UI.Combobox;
using SysMgt.BussnessDTOModule.Combobox;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule
{
    public class SetupRepository : DBSqlRepositoryBase<BDTA_SETUP>, ISetupRepository
    {
        public SetupRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }
        
        public PageList<SetupPageResult> GetPageList(SetupSearchModel search)
        {
            var contextTable = GetContextTable();
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.SetupType))
                {
                    contextTable = contextTable.Where((BDTA_SETUP setup) => setup.SETUP_TYPE == search.SetupType);
                }
            }
            return contextTable.Paging((BDTA_SETUP t) => new SetupPageResult
            {
                ID = t.Id,
                SetupNo = t.SETUP_NO,
                SetupContents = t.SETUP_CONTENTS,
                SetupParameter=t.SETUP_PARAMETER
            }, search.page);
        }

        public PageList<ComboboxData> GetComboxListByKey(SearchModel search,string key)
        {
            return GetContextTable().Where((BDTA_SETUP t) => t.SETUP_KEY == key).Paging((BDTA_SETUP t) => new ComboboxData
            {
                Id = t.SETUP_NO,
                Text = t.SETUP_CONTENTS
            }, search.page);
        }

        public PageList<ComboboxData> GetComboxListByType(SearchModel search, string type)
        {
            return GetContextTable().Where((BDTA_SETUP t) => t.SETUP_TYPE == type).Paging((BDTA_SETUP t) => new ComboboxData
            {
                Id = t.SETUP_NO,
                Text = t.SETUP_CONTENTS
            }, search.page);
        }
    }
}
