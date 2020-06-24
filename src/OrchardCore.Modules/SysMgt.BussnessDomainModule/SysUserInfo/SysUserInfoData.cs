using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.SysUserInfo
{
   public class SysUserInfoData 
    {
        public string ID { get; set; }
        public string UserCode { get; set; }
        public string CName { get; set; }
        public string Password { get; set; }
        public long? State { get; set; }
        public string Memo { get; set; }
    }
}
