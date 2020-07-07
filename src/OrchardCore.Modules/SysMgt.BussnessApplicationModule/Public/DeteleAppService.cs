//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BlocksCore.Application.Abstratctions;
//using BlocksCore.Data.Abstractions.Paging;
//using Microsoft.Extensions.Localization;
//using SysMgt.BussnessDomainModule.Public;
//using SysMgt.BussnessDTOModule.Public;



//namespace SysMgt.BussnessApplicationModule.Public
//{
//    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“DeteleAppService”。
//    public class DeteleAppService : AppService, IDeteleAppService
//    {
//        public DeleteDomain DeleteDomain { get; set; }

//        public DeteleAppService(DeleteDomain deleteDomain) {

//            this.DeleteDomain = deleteDomain;
//        }
//        public string Delete(DeleteInfo info)
//        {
           
//            return DeleteDomain.Delete(info);
//        }

     
//    }
//}
