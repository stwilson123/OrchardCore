//using BlocksCore.Domain.Abstractions.Domain;
//using BlocksCore.Domain.Abstractions;
//using Microsoft.Extensions.Localization;
//using BlocksCore.Abstractions.Security;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;

//namespace SysMgt.BussnessDomainModule.Common
//{
//    public class UpLoadFileDomain : IDomainService
//    {
//        public IUserContext UserContext { get; set; }

//        public UpLoadFileDomain(IUserContext UserContext)
//        {
//            this.UserContext = UserContext;
//        }

//        public string UpLoadFile()
//        {
//            try
//            {
//                HttpRequest request = HttpContext.Current.Request;
//                HttpFileCollection fileCollection = request.Files;
//                if (request.Files.Count == 0)
//                {
//                    throw new BlocksBussnessException("101", L["没有要上传的文件"], null);
//                }
//                var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"/upload/");
//                for (int i = 0; i < request.Files.Count; i++)
//                {
//                    fileCollection[i].SaveAs(Path.Combine(filePath, fileCollection[i].FileName));
//                }
//                return Path.Combine(filePath, fileCollection[0].FileName);
//            }

//            catch (Exception ex)
//            {
//                throw new BlocksBussnessException("101", L(ex.Message), null);
//            }
//        }
//    }

  
//}
