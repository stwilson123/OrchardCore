using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.Employee
{
    public class EmployeeData
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string EmpType { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string combobox { get; set; }//部门Id
    }
}
