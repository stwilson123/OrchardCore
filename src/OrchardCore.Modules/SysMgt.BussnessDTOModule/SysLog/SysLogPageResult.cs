using BlocksCore.Abstractions.Datatransfer;
using System;

namespace SysMgt.BussnessDTOModule.SysLog
{
    
    public class SysLogPageResult 
    {
        public string ID { get; set; }
        public string ModuleName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public DateTime LogDate { get; set; }
        public string LogLevel { get; set; }
        public string LogException { get; set; }
        public string UserId { get; set; }
        public decimal TakeTime { get; set; }
        public string IPAddress { get; set; }
        public string ComputerName { get; set; }
        public string CustomMessage { get; set; }
    }
}
