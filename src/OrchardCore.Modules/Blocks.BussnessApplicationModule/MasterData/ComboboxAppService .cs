using System;
using Blocks.BussnessDomainModule;
using Blocks.BussnessDomainModule.MasterData;
using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.MasterData;
using BlocksCore.Application.Abstratctions;
using BlocksCore.AutoMapper.Abstractions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;


namespace Blocks.BussnessApplicationModule.MasterData
{
    public class ComboboxAppService : AppService,IComboboxAppService
    {
        public ComboboxAppService(ComboboxDomainEvent comboboxDomain)
        {
            this.ComboboxDomain = comboboxDomain;
        }

        private ComboboxDomainEvent ComboboxDomain { get; set; }
        
        public  PageList<ComboboxData>  GetComboboxList(SearchModel a)
        {
            return ComboboxDomain.GetComboboxList(a);
        }

    
    }
}