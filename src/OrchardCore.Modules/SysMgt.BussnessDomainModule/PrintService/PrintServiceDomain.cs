//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BlocksCore.Domain.Abstractions.Domain;
//using Blocks.BussnessEntityModule;
//using BlocksCore.Abstractions.UI.Combobox;
//using BlocksCore.Data.Abstractions.Paging;
//using BlocksCore.Domain.Abstractions;
//using Microsoft.Extensions.Localization;
//using SysMgt.BussnessDTOModule.Combobox;
//using SysMgt.BussnessDTOModule.PrintService;
//using BarTender;

//using SysMgt.BussnessRespositoryModule;
//using Newtonsoft.Json.Linq;
//using SysMgt.BussnessDomainModule.Common;
//using Microsoft.Extensions.Logging;
//using System.Text.RegularExpressions;

//namespace SysMgt.BussnessDomainModule.PrintService
//{
//    public class PrintServiceDomain : IDomainService
//    {
//        public IPrintServiceRepository PrintServiceRepository { get; set; }
//        private ISetupRepository SetupRepository { get; set; }
//        private ILogger log;

//        public PrintServiceDomain(IPrintServiceRepository printServiceRepository, ILogger<PrintServiceDomain> log, ISetupRepository setupRepository)
//        {
//            this.PrintServiceRepository = printServiceRepository;
//            this.log = log;
//            this.SetupRepository = setupRepository;
//        }


//        public virtual string AddWhReceive(PrintServiceData printServiceData)
//        {

//            PRINT_SERVICE printService = new PRINT_SERVICE();
//            printService.Id = printServiceData.ID;
//            printService.PRINTTEMPLATE_PATH = printServiceData.Path;
//            printService.PRINT_FLAG = printServiceData.Flag;
//            printService.PRINT_TYPE = printServiceData.Type;
//            printService.PRINT_NAME = printServiceData.Name;
//            printService.PRINT_CONTENT_JSONTXT = printServiceData.JsonTxt;
//            string returntId = PrintServiceRepository.InsertAndGetId(printService);            
//            if (string.IsNullOrEmpty(returntId))
//            {
//                return "打印失败";
//            }
//            else
//            {
//                //return Print(printService);
//                return "打印成功";
//            }
//        }

//        private string Print(PRINT_SERVICE printService)
//        {
//                    var list = JObject.Parse(printService.PRINT_CONTENT_JSONTXT.ToString())["list"];
//                    string Json = printService.PRINT_CONTENT_JSONTXT.ToString();
//                    JObject jobj = JObject.Parse(Json);
//                    string num = string.Empty;

//                    if (Json.Contains("Copies"))
//                    {
//                        num = jobj["Copies"].ToString();////设置同序列打印的份数  
//                    }
//                    else
//                    {
//                        num = "1";
//                    }

//                    string path = printService.PRINTTEMPLATE_PATH;      //模板
//                    string printName = printService.PRINT_NAME;         //打印机

//            var exsitModelData = SetupRepository.FirstOrDefault(t => t.SETUP_NO == printService.PRINTTEMPLATE_PATH);
//            if (exsitModelData == null)
//            {
//                return "未找到对应模板WhReceievePrint的系统路径配置，请配置后再打印";
//            }
//            else {
//                path = exsitModelData.SETUP_PARAMETER;
//            }

//            var exsitPrinterData = SetupRepository.FirstOrDefault(t => t.SETUP_NO == printService.PRINT_NAME);
//            if (exsitPrinterData == null)
//            {
//                return "未找到对应打印机"+ printService.PRINT_NAME + "的系统路径配置，请配置后再打印";
//            }
//            else
//            {
//                printName = exsitPrinterData.SETUP_PARAMETER;
//            }


//            try
//            {
//                BarTender.Application btApp = new BarTender.Application();
//                Format btFormat = new Format();
//                btFormat = btApp.Formats.Open(@path + ".btw", false, @printName);  //打开模版文件

//                foreach (var item in list)
//                    {
//                            btFormat.PrintSetup.IdenticalCopiesOfLabel = int.Parse(num);  //设置同序列打印的份数  
//                            //向bartender模板传递变量  
//                            btFormat.SetNamedSubStringValue("txt_lotno", item["lotno"].ToString()); //批次
//                            btFormat.SetNamedSubStringValue("txt_matcode", item["matcode"].ToString()); //物料Code
//                            btFormat.SetNamedSubStringValue("txt_qty", item["qty"].ToString()); //数量

//                    int x = btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性
//                    //Messages a;
//                    //btFormat.Print("123",true,-1,out a);
//                    if (x == 1) {
//                        return "物料："+ item["matcode"].ToString()+",批次："+ item["lotno"].ToString() + "，打印机："+ @printName + "打印失败,中止打印。";
//                    }
//                }
//                btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges); //关闭模版文件
//                btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);  //关闭文件流
//            }
//            catch (Exception ex)
//            {
//                //throw new BlocksBussnessException("101", L["无法找到打印模板或打印机"], null);
//                //return "无法找到打印模板或打印机！";
//                log.Logger(new LogModel()
//                {
//                    Message = "调用打印服务："+ex.Message
//                });
//                return "打印有误，"+ex.Message;
//            }

//            return "打印成功";

//        }

//        public List<PrintServiceRetun> GetPrintList()
//        {
//            //查询待打印列表，翻译对于打印模板位置及对应打印机路径
//            List<PrintServiceRetun> returnList = new List<PrintServiceRetun>();

//            var printServiceList = PrintServiceRepository.GetAllList(t => t.PRINT_FLAG == 0);
            
//            foreach (var printService in printServiceList)
//            {
//                var updateQty = 0;
//                PrintServiceRetun psreturn = new PrintServiceRetun();
//                var exsitModelData = SetupRepository.FirstOrDefault(t => t.SETUP_NO == printService.PRINTTEMPLATE_PATH);
//                if (exsitModelData == null)
//                {
//                    // return "未找到对应模板WhReceievePrint的系统路径配置，请配置后再打印";
//                    updateQty = PrintServiceRepository.Update(t => t.Id == printService.Id,
//                    t => new PRINT_SERVICE()
//                    {
//                        PRINT_FLAG = 2,
//                        UPDATER = "dac0c8cd-26a0-4c3c-bc6e-5770ef5d8e87"
//                    }
//                    );
//                    if (updateQty <= 0)
//                    {
//                        throw new BlocksBussnessException("101", L["更新打印数据标记失败"], null);
//                    }
//                    continue;
//                }
//                else
//                {
//                    psreturn.PrintPath = exsitModelData.SETUP_PARAMETER;
//                    psreturn.PrintType = printService.PRINTTEMPLATE_PATH;
//                }

//                var exsitPrinterData = SetupRepository.FirstOrDefault(t => t.SETUP_NO == printService.PRINT_NAME);
//                if (exsitPrinterData == null)
//                {
//                    // return "未找到对应打印机" + printService.PRINT_NAME + "的系统路径配置，请配置后再打印";
//                    updateQty = PrintServiceRepository.Update(t => t.Id == printService.Id,
//                    t => new PRINT_SERVICE()
//                    {
//                        PRINT_FLAG = 2,
//                        UPDATER = "dac0c8cd-26a0-4c3c-bc6e-5770ef5d8e87"
//                    }
//                    );
//                    if (updateQty <= 0)
//                    {
//                        throw new BlocksBussnessException("101", L["更新打印数据标记失败"], null);
//                    }
//                    continue;
//                }
//                else
//                {
//                    psreturn.PrintName = exsitPrinterData.SETUP_PARAMETER;
//                }
                
//                psreturn.PRINT_CONTENT_JSONTXT = Regex.Unescape(printService.PRINT_CONTENT_JSONTXT);
//                returnList.Add(psreturn);

//                updateQty = PrintServiceRepository.Update(t => t.Id == printService.Id,
//                t => new PRINT_SERVICE()
//                {
//                    PRINT_FLAG = 1,
//                    UPDATER = "dac0c8cd-26a0-4c3c-bc6e-5770ef5d8e87"
//                }
//                );
//                if (updateQty <= 0)
//                {
//                    throw new BlocksBussnessException("101", L["更新打印数据标记失败"], null);
//                }
//            }
//                return returnList;
//        }
//    }
//}