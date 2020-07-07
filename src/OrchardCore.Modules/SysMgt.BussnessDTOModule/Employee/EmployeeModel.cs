using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Abstractions.Datatransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.Employee
{
    /// <summary>
    /// 搜索使用
    /// </summary>
    public class EmployeeSearchModel 
    {

        public string Code { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public Page page { get; set; }

    }

    /// <summary>
    /// 新增或编辑使用-只能Application层使用
    /// </summary>
    public class EmployeeInfo 
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

    /// <summary>
    /// 分页返回的查询数据
    /// </summary>
    public class EmployeePageResult 
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string EmpType { get; set; }
        public string EmpTypeName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DeptName { get; set; }
    }

}
