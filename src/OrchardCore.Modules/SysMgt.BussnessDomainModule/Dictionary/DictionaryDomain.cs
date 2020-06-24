using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Domain.Abstractions.Domain;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessRespositoryModule;

namespace SysMgt.BussnessDomainModule.Dictionary
{
    public class DictionaryDomain : IDomainService
    {

        /// <summary>
        /// 申明接口
        /// </summary>
        private IDictionaryRepository DictionaryRepository { get; set; }
        public DictionaryDomain(IDictionaryRepository DictionaryRepository)
        {
            this.DictionaryRepository = DictionaryRepository;
        }
        public virtual PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return DictionaryRepository.GetComboxList(search);
        }
    }
}
