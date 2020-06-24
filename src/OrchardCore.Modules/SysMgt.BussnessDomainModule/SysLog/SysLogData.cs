using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.SysLog
{
  public class SysLogData  
    {
        //public string ID { get; set; }
        public string ModuleName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        //public DateTime LogDate { get; set; }
        public string LogLevel { get; set; }
        public string LogException { get; set; }
        //public string UserId { get; set; }
        public TimeSpan TakeTime { get; set; }
        //public string IPAddress { get; set; }
        //public string ComputerName { get; set; }
        public string CustomMessage { get; set; }
    }
}
