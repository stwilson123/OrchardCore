using BlocksCore.Application.Abstratctions;
using SysMgt.BussnessDTOModule.PrintService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessApplicationModule.PrintService
{
    public interface IPrintServiceAppService : IAppService
    {
        string Add(PrintServiceInfo printServiceInfo);
        List<PrintServiceRetun> GetPrintList(PrintServiceInfo printServiceInfo);
        //HttpResponseMessage GetPrintList(PrintServiceInfo printServiceInfo);
    }
}
