using BlocksCore.Application.Abstratctions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.ConfigFiles;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.ConfigFiles;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;


namespace SysMgt.BussnessApplicationModule.ConfigFiles
{

    public class ConfigFilesAppService : AppService, IConfigFilesAppService
    {

        private ConfigFilesDomain configFilesDomain { get; set; }
        public ConfigFilesAppService(ConfigFilesDomain configFilesDomain)
        {
            this.configFilesDomain = configFilesDomain;
        }
        public PageList<ConfigFilesPageResult> GetPageList(ConfigFilesSearchModel search)
        {
            return configFilesDomain.GetPageList(search);
        }

        public ConfigFilesInfo GetOneById(ConfigFilesInfo pInfo)
        {

            return configFilesDomain.GetOneById(pInfo);
        }

        public string Add(ConfigFilesInfo pInfo)
        {
            return configFilesDomain.Add(pInfo);
        }
        public string Update(ConfigFilesInfo pInfo)
        {
            return configFilesDomain.Update(pInfo);
        }


        public string Delete(CommonEntity pInfo)
        {
            return configFilesDomain.Delete(pInfo);
        }



        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        /// 

        //public string UploadFile()
        //{
        //    //接收附件
        //    HttpRequest request = HttpContext.Current.Request;
        //    HttpFileCollection fileCollection = request.Files;
        //    if (request.Files.Count < 1) {
        //        throw new BlocksBussnessException("101", StringLocal.Format("未发现上传的文件"), null);
        //    }
        //    //接收前端参数
        //    var ftype = request.Params["FileType"];
        //    if (ftype == null) {
        //        throw new BlocksBussnessException("101", StringLocal.Format("上传失败，没找到文件类型"), null);
        //    }
        //    //var fpath = "/upload/" + ftype + "/";
        //    //var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@fpath);//HttpContext.Current.Server.MapPath()
        //    //相对路径转成物理路径
        //    var filePath = urlTolocal(ftype);
        //    //文件夹如果不存在，自动创建
        //    if (Directory.Exists(filePath) == false) {
        //        Directory.CreateDirectory(filePath);
        //    }

        //    //物理路径转换成URL路径
        //    //var urlpath = urlconvertor(filePath);

        //    for (int i = 0; i < request.Files.Count; i++)
        //    {
        //        fileCollection[i].SaveAs(Path.Combine(filePath, fileCollection[i].FileName));
        //    }
        //    return filePath;

        //}

        ///// <summary>
        ///// //物理路径转换成相对路径
        ///// </summary>
        ///// <param name="imagesurl1">物理路径</param>
        ///// <returns></returns>
        //private string urlconvertor(string imagesurl1)
        //{
        //    string tmpRootDir = System.Web.Hosting.HostingEnvironment.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
        //    string imagesurl2 = imagesurl1.Replace(tmpRootDir, ""); //转换成相对路径
        //    imagesurl2 = imagesurl2.Replace(@"\", @"/");
        //    //imagesurl2 = imagesurl2.Replace(@"Aspx_Uc/", @"");      
        //    return imagesurl2;
        //}

        //private string urlTolocal(string imagesurl1)
        //{
        //    string tmpRootDir = HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录  
        //    string imagesurl2 = tmpRootDir + imagesurl1.Replace(@"/", @"\"); //转换成绝对路径  
        //    return imagesurl2;
        //}



        public void downfile(ConfigFilesInfo configFilesInfo)
        {
            WebClient client = new WebClient();
            string url = configFilesInfo.FilePath + configFilesInfo.FileName; 
            string dir = configFilesInfo.FileName;
            string path =  configFilesInfo.FileName;
            client.DownloadFile(url, "d:\\"+path);
        }
    }
}
