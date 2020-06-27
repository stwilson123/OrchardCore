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
using BlocksCore.Data.Abstractions.Transaction;
using Microsoft.AspNetCore.Authorization;

namespace SysMgt.BussnessApplicationModule
{
    [Transaction(false)]
    public class DictionaryAppService : AppService, IDictionaryAppService
    {
        private DictionaryDomain dictionaryDomain { get; set; }
        public DictionaryAppService(DictionaryDomain dictionaryDomain)
        {
            this.dictionaryDomain = dictionaryDomain;
        }
        [Authorize]
        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return dictionaryDomain.GetComboxList(search);
        }
    }
}
