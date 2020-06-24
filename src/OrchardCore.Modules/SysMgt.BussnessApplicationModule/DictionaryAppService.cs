using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.Dictionary;
using SysMgt.BussnessDTOModule.Combobox;


namespace SysMgt.BussnessApplicationModule
{
    public class DictionaryAppService : AppService, IDictionaryAppService
    {
        private DictionaryDomain dictionaryDomain { get; set; }
        public DictionaryAppService(DictionaryDomain dictionaryDomain)
        {
            this.dictionaryDomain = dictionaryDomain;
        }
        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return dictionaryDomain.GetComboxList(search);
        }
    }
}
