//using BlocksCore.Application.Abstratctions;
//using BlocksCore.AutoMapper.Abstractions;
//using BlocksCore.Data.Abstractions.Paging;
//using SysMgt.BussnessDomainModule.SysGlobal;
//using SysMgt.BussnessDTOModule.Common;
//using SysMgt.BussnessDTOModule.SysGlobal;

//namespace SysMgt.BussnessApplicationModule.SysGlobal
//{

//    public class SysGlobalAppService : AppService, ISysGlobalAppService
//    {
//        private SysGlobalDomain SysGlobalDomain { get; set; }
//        public SysGlobalAppService(SysGlobalDomain sysGlobalDomain)
//        {
//            this.SysGlobalDomain = sysGlobalDomain;
//        }

//        public SysGlobalInfo CustomerXml(SysGlobalInfo sysGlobalInfo)
//        {
//            SysGlobalData sysGlobalData = sysGlobalInfo.AutoMapTo<SysGlobalData>();
//            return SysGlobalDomain.CustomerXml(sysGlobalData).AutoMapTo<SysGlobalInfo>();
//        }
//    }
//}
