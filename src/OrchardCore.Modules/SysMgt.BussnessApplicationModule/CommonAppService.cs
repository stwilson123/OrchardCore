//using BlocksCore.Application.Abstratctions;
//using SysMgt.BussnessDomainModule.Common;
//using SysMgt.BussnessDTOModule.Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;

//namespace SysMgt.BussnessApplicationModule
//{
//    public class CommonAppService : AppService, ICommonAppService
//    {

//        private ImportExplainDomain importExplainDomain { get; set; }

//        private UpLoadFileDomain upLoadFileDomain { get; set; }
//        public CommonAppService(ImportExplainDomain importExplainDomain, UpLoadFileDomain upLoadFileDomain)
//        {
//            this.importExplainDomain = importExplainDomain;
//            this.upLoadFileDomain = upLoadFileDomain;
//        }
//        public string GetImportConfigInfo(CommonEntity pInfo)
//        { 
//            return importExplainDomain.GetImportConfigInfo(pInfo); 
//        }
//        public string ImportData(CommonEntity pInfo)
//        {
//            return importExplainDomain.ImportData(pInfo);
//        }

//        public string UploadFile()
//        {  
//            return upLoadFileDomain.UpLoadFile(); 
//        }
//    }
//}
