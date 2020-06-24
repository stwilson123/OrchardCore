//using BlocksCore.Application.Abstratctions;
//using Blocks.Framework.Tools.Json;
//using SysMgt.BussnessDomainModule.PrintService;
//using SysMgt.BussnessDTOModule.PrintService;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web;
//using System.Text;
//using System.Threading.Tasks;

//namespace SysMgt.BussnessApplicationModule.PrintService
//{
//    public class PrintServiceAppService : AppService, IPrintServiceAppService
//    {
//        public PrintServiceDomain printServiceDomain { get; set; }

//        public PrintServiceAppService(PrintServiceDomain printServiceDomain)
//        {
//            this.printServiceDomain = printServiceDomain;
//        }
//        public string Add(PrintServiceInfo printServiceInfo)
//        {
//            string masterplate = printServiceInfo.Path; //唯一标识码 
//            string equipmentCode = "";
//            string count = printServiceInfo.Copies;
//            List<Object> list = printServiceInfo.PrintList;

//            //foreach (var item in printServiceInfo.PrintList)
//            //{
//            //    list.Add(new
//            //    {
//            //        lotno = item
//            //    });

//            //}

//            object obj = new
//            {
//                masterplate,
//                equipmentCode,//设备编码
//                Copies = count,
//                list
//            };
//            string printJson = JsonHelper.SerializeObject(obj);

//            //a从printServiceInfo.PrintList转成json格式字符串再给Data赋值
//            PrintServiceData printServiceData = new PrintServiceData();
//            printServiceData.ID = printServiceInfo.ID;
//            printServiceData.Path = printServiceInfo.Path;
//            printServiceData.Name = printServiceInfo.Name;
//            printServiceData.Flag = printServiceInfo.Flag;
//            printServiceData.Type = printServiceInfo.Type;
//            printServiceData.JsonTxt = printJson;

//            switch (masterplate)
//            {
//                case "WhReceievePrint":
//                    return printServiceDomain.AddWhReceive(printServiceData);
//                default:
//                    return "此模板未在程序中配置";
//            }
//            //return printServiceDomain.Add(printServiceData);
//        }
//        public List<PrintServiceRetun> GetPrintList(PrintServiceInfo printServiceInfo)
//        {
//            return printServiceDomain.GetPrintList();
//        }

//        //public HttpResponseMessage GetPrintList(PrintServiceInfo printServiceInfo)
//        //{
//        //    HttpResponseMessage a = new HttpResponseMessage();
//        //    string postData = "{ ID: \"1\" }";
//        //    //a.Content = new StringContent("12345");
//        //    a.Content = new StringContent(postData);
//        //    a.StatusCode = HttpStatusCode.NotFound;
//        //    return a;
//        //}

//    }
//}
