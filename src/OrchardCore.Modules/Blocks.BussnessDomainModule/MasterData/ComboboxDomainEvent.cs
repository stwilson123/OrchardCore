using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.MasterData;
using Blocks.BussnessEntityModule;
using Blocks.BussnessRespositoryModule;
using BlocksCore.Data.Abstractions.Paging;
using Blocks.Framework.Utility.TypeHelper;
using BlocksCore.Domain.Abstractions.Domain;
using BlocksCore.Abstractions.UI.Combobox;

namespace Blocks.BussnessDomainModule.MasterData
{
    public class ComboboxDomainEvent : IDomainService
    {
        public ComboboxDomainEvent(ITest2Repository testRepository)
        {
              this.Test2Repository = testRepository;
        }

        private ITest2Repository Test2Repository { get; set; }
        
        
        public virtual  PageList<ComboboxData>  GetComboboxList(SearchModel search)
        {
            var a =  Test2Repository.GetPageList(search);
            return a;
        }
    }
}