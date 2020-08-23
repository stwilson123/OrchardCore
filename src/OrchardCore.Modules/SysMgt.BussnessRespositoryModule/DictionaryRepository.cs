using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;

using BlocksCore.Data.Linq2DB.Repository;
using SysMgt.BussnessDTOModule.Combobox;
using BlocksCore.Data.Linq;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule
{
    public class DictionaryRepository : DBSqlRepositoryBase<BDTA_DICTIONARY>, IDictionaryRepository
    {
        public DictionaryRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }
        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return GetContextTable()
                .InnerJoin((BDTA_DICTIONARY t) => t.DIC_TYPE_ID,(BDTA_DICTIONARY_TYPE type)=> type.Id)
                .Where((BDTA_DICTIONARY_TYPE type) => type.DIC_TYPE_NO == search.DictionaryTypeCode)
                .Paging((BDTA_DICTIONARY t) => new ComboboxData
            {
                Id = t.Id,
                Text = t.DIC_VALUE
            }, search.page);
        }

    }
}
