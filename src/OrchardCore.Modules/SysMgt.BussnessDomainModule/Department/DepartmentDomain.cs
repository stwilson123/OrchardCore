using BlocksCore.Domain.Abstractions.Domain;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using Microsoft.Extensions.Localization;
using SysMgt.BussnessDomainModule.Common;
using SysMgt.BussnessDTOModule;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessRespositoryModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.Department
{
    public class DepartmentDomain : IDomainService
    {

        /// <summary>
        /// 申明接口
        /// </summary>
        private IDepartmentRepository departmentRepository { get; set; }

        public IStringLocalizer L { get; set; }
        /// <summary>
        /// 构造函数,实例化对象
        /// </summary>
        /// <param name="departmentRepository"></param>
        public DepartmentDomain(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        public virtual PageList<DepartmentPageResult> GetPageList(DepartmentSearchModel search)
        {
            return departmentRepository.GetPageList(search);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="departmentData"></param>
        /// <returns></returns>
        public virtual string Add(DepartmentData departmentData)
        {

            #region 解析json数据，并且赋值对象


            BDTA_DEPARTMENT model = new BDTA_DEPARTMENT();
            model.Id = Guid.NewGuid().ToString();
            if (departmentData.Code == "")
            {
                HelperBLL.ThrowEx("101", L["编码不能为空！"]);
            }
            model.DEPARTMENT_NO = departmentData.Code;
            if (departmentData.Name == "")
            {
                HelperBLL.ThrowEx("101", L["名称不能为空！"]);
            }
            model.DEPARTMENT_NAME = departmentData.Name;
            model.DEPARTMENT_DESC = departmentData.Desc;
            #endregion

            #region 判断主目录是否已存在

            var curEntity = departmentRepository.FirstOrDefault(t => t.DEPARTMENT_NO == model.DEPARTMENT_NO);
            if (curEntity != null)
            {
                HelperBLL.ThrowEx("101", L["编码已存在！"]);
            }

            #endregion

            #region 新增

            var isSuccessed = departmentRepository.InsertAndGetId(model);
            if (string.IsNullOrEmpty(isSuccessed))
            {
                return "保存失败";
            }
            else
            {
                return "保存成功！";
            }
            

            #endregion



        }

        /// <summary>
        /// 获得下拉框的值
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public virtual PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return departmentRepository.GetComboxList(search);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="outInTypeData"></param>
        /// <returns></returns>
        public virtual string Update(DepartmentData departmentData)
        {


            #region 判断主目录是否已存在

            var curEntity = departmentRepository.FirstOrDefault(t => t.DEPARTMENT_NO == departmentData.Code && t.Id != departmentData.ID);
            if (curEntity != null)
            {
                HelperBLL.ThrowEx("101", L["编码已存在！"]);
            }

            #endregion

            #region 编辑
            if (departmentData.Code == "")
            {
                HelperBLL.ThrowEx("101", L["编码不能为空！"]);
            }
            if (departmentData.Name == "")
            {
                HelperBLL.ThrowEx("101", L["名称不能为空！"]);
            }
            int successCount = departmentRepository.Update(t => t.Id == departmentData.ID, t => new BDTA_DEPARTMENT()
            {
               
                DEPARTMENT_NO = departmentData.Code,
                DEPARTMENT_NAME = departmentData.Name,
                DEPARTMENT_DESC = departmentData.Desc,

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
            
            departmentRepository.Delete(t => ids.Contains(t.Id) && t.ISUSED != 1);
            return "删除成功！";

        }

        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="departmentData"></param>
        /// <returns></returns>
        public virtual DepartmentData GetOneById(DepartmentData departmentData)
        {
            var model = departmentRepository.FirstOrDefault(t => t.Id == departmentData.ID);
            if (model == null)
            {
                HelperBLL.ThrowEx("101", L["未查到对象！"]);
            }

            return new DepartmentData()
            {

                ID = model.Id,
                Code = model.DEPARTMENT_NO,
                Name = model.DEPARTMENT_NAME,
                Desc = model.DEPARTMENT_DESC

            };

        }


    }
}
