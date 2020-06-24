using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.ThirdSystemType;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.ThirdSystemType;
using System.Collections.Generic;

namespace SysMgt.BussnessApplicationModule.ThirdSystemCall
{
   
    public class ThirdSystemTypeAppService : AppService, IThirdSystemTypeAppService
    {

        private ThirdSystemTypeDomain thirdSystemTypeDomain { get; set; }
        public ThirdSystemTypeAppService(ThirdSystemTypeDomain thirdSystemTypeDomain)
        {
            this.thirdSystemTypeDomain = thirdSystemTypeDomain;
        }
        public PageList<ThirdSystemTypePageResult> GetPageList(ThirdSystemTypeSearchModel search)
        {
            return thirdSystemTypeDomain.GetPageList(search);
        }

        public ThirdSystemTypeInfo GetOneById(ThirdSystemTypeInfo pInfo)
        {
           
            return thirdSystemTypeDomain.GetOneById(pInfo);
        }

        public string Add(ThirdSystemTypeInfo pInfo)
        { 
            return thirdSystemTypeDomain.Add(pInfo);
        } 

        public string Update(ThirdSystemTypeInfo pInfo)
        {            
            return thirdSystemTypeDomain.Update(pInfo);
        }

        public string Delete(CommonEntity pInfo)
        {
            return thirdSystemTypeDomain.Delete(pInfo);
        }

        public List<ComboboxData> GetComboxList()
        {
            return thirdSystemTypeDomain.GetComboxList();
        }
    }
}
