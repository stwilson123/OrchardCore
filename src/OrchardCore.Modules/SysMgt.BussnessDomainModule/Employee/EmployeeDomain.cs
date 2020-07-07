using BlocksCore.Domain.Abstractions.Domain;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.Localization;
using SysMgt.BussnessDomainModule.Common;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.Employee;
using SysMgt.BussnessRespositoryModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.Employee
{
    public class EmployeeDomain : IDomainService
    {

        /// <summary>
        /// 申明接口
        /// </summary>
        private IEmployeeRepository employeeRepository { get; set; }

        public IStringLocalizer L { get; set; }
        /// <summary>
        /// 构造函数,实例化对象
        /// </summary>
        /// <param name="departmentRepository"></param>
        public EmployeeDomain(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        public virtual PageList<EmployeePageResult> GetPageList(EmployeeSearchModel search)
        {

            PageList<EmployeePageResult> employeePageResults = employeeRepository.GetPageList(search);
            foreach (EmployeePageResult employeePageResult in employeePageResults.Rows)
            {
                //职员类型标记 A 正式员工 T 临时员工 Q 离职员工
                if (employeePageResult.EmpType == "A")
                {
                    employeePageResult.EmpTypeName = "正式员工";
                }
                if (employeePageResult.EmpType == "T")
                {
                    employeePageResult.EmpTypeName = "临时员工";
                }
                if (employeePageResult.EmpType == "Q")
                {
                    employeePageResult.EmpTypeName = "离职员工";
                }
            }

            return employeePageResults;


        }

        public  string Add(EmployeeData employeeData)
        {
            if (employeeData.Code == "")
            {
                HelperBLL.ThrowEx("101", L["编码不能为空！"]);
            }
            if (employeeData.Name == "")
            {
                HelperBLL.ThrowEx("101", L["名称不能为空！"]);
            }
            if (employeeData.EmpType == "")
            {
                HelperBLL.ThrowEx("101", L["类型不能为空！"]);
            }
            if (employeeData.combobox == "")
            {
                HelperBLL.ThrowEx("101", L["所属部门不能为空！"]);
            }
            #region 解析json数据，并且赋值对象

            BDTA_EMPLOYEE model = new BDTA_EMPLOYEE();
            model.Id = Guid.NewGuid().ToString();
            model.EMPLOYEE_NO = employeeData.Code;
            model.EMPLOYEE_NAME = employeeData.Name;
            model.EMPLOYEE_DESC = employeeData.Desc;
            model.EMPLOYEE_TYPE = employeeData.EmpType;
            model.EMPLOYEE_EMAIL = employeeData.Email;
            model.EMPLOYEE_PHONE = employeeData.Phone;
            model.DEPARTMENT_ID = employeeData.combobox;
            model.DATAVERSION = 0;  //数据版本号
            #endregion

            #region 判断主目录是否已存在

            var curEntity = employeeRepository.FirstOrDefault(t => t.EMPLOYEE_NO == model.EMPLOYEE_NO);
            if (curEntity != null)
            {
                HelperBLL.ThrowEx("101", L["编码已存在！"]);
            }

            #endregion

            #region 新增

            var isSuccessed = employeeRepository.InsertAndGetId(model);
            if (string.IsNullOrEmpty(isSuccessed))
            {
                //HelperBLL.ThrowEx("101", L["保存失败！"]);
                return "保存失败";
            }
            else
            {
                return "保存成功！";
            }

            #endregion



        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="employeeData"></param>
        /// <returns></returns>
        public virtual string Update(EmployeeData employeeData)
        {


            #region 判断主目录是否已存在

            var curEntity = employeeRepository.FirstOrDefault(t => t.EMPLOYEE_NO == employeeData.Code && t.Id != employeeData.ID);
            if (curEntity != null)
            {
                HelperBLL.ThrowEx("101", L["编码已存在！"]);
            }

            #endregion

            #region 编辑

            int successCount = employeeRepository.Update(t => t.Id == employeeData.ID, t => new BDTA_EMPLOYEE()
            {
                EMPLOYEE_NO = employeeData.Code,
                EMPLOYEE_NAME = employeeData.Name,
                EMPLOYEE_DESC = employeeData.Desc,
                EMPLOYEE_TYPE = employeeData.EmpType,
                EMPLOYEE_EMAIL = employeeData.Email,
                EMPLOYEE_PHONE = employeeData.Phone,
                DEPARTMENT_ID = employeeData.combobox,

            });
            if (successCount > 0)
            {
                return "更新成功";
            }
            else
            {
                return "更新失败";
            }

            #endregion

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual string Delete(List<string> ids)
        {
            employeeRepository.Delete(t => ids.Contains(t.Id) && t.ISUSED != 1);
            return "删除成功！";
        }

        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="employeeData"></param>
        /// <returns></returns>
        public virtual EmployeeData GetOneById(EmployeeData employeeData)
        {
            var model = employeeRepository.FirstOrDefault(t => t.Id == employeeData.ID);
            if (model == null)
            {
                HelperBLL.ThrowEx("101", L["未查到对象！"]);
            }

            return new EmployeeData()
            {
                ID = model.Id,
                Code = model.EMPLOYEE_NO,
                Name = model.EMPLOYEE_NAME,
                Desc = model.EMPLOYEE_DESC,
                EmpType = model.EMPLOYEE_TYPE,
                Email = model.EMPLOYEE_EMAIL,
                Phone = model.EMPLOYEE_PHONE,
                combobox = model.DEPARTMENT_ID
            };

        }

        public virtual PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return employeeRepository.GetComboxList(search);
        }

    }
}
