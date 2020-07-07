using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.Combobox;
using Blocks.BussnessDTOModule.MasterData;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Data.EF.Linq;
using BlocksCore.Data.EF.Repository;

namespace Blocks.BussnessRespositoryModule
{
    public class Test2Repository : DBSqlRepositoryBase<TESTENTITY2>, ITest2Repository
    {
        public Test2Repository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        
      
        PageList<ComboboxData> ITest2Repository.GetPageList(SearchModel search)
        {
            return GetContextTable().Paging((TESTENTITY2 t) => new ComboboxData{
                Id = t.Id,
                Text = t.Text
            }, search.page);
        }
    }
}