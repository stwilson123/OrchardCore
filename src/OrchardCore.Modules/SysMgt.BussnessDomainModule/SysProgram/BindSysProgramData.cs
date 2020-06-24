using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.SysProgram
{
    public class BindSysProgramData
    {
        public string ID { get; set; }

		public int Platform { get; set; }

		public List<SysProgramData> ListsSysProgramDatas { get; set; }
    }
}
