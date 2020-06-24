using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.ThirdSystemCall;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.ThirdSystemCall;

namespace SysMgt.BussnessApplicationModule.ThirdSystemCall
{

    public class ThirdSystemCallAppService : AppService, IThirdSystemCallAppService
    {

        private ThirdSystemCallDomain thirdSystemCallDomain { get; set; }
        public ThirdSystemCallAppService(ThirdSystemCallDomain thirdSystemCallDomain)
        {
            this.thirdSystemCallDomain = thirdSystemCallDomain;
        }
        public PageList<ThirdSystemCallPageResult> GetPageList(ThirdSystemCallSearchModel search)
        {
            return thirdSystemCallDomain.GetPageList(search);
        }

        public ThirdSystemCallInfo GetOneById(ThirdSystemCallInfo pInfo)
        {

            return thirdSystemCallDomain.GetOneById(pInfo);
        }

        public string Update(ThirdSystemCallInfo pInfo)
        {
            return thirdSystemCallDomain.Update(pInfo);
        }

        public string ReCall(CommonEntity pInfo)
        {
            return thirdSystemCallDomain.ReCall(pInfo);
        }
    }
}
