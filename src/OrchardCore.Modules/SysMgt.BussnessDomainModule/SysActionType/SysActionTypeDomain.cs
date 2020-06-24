using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Domain.Abstractions.Domain;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.SysUserInfo;
using SysMgt.BussnessDTOModule.SysActionType;
using SysMgt.BussnessDTOModule.SysUserInfo;
using SysMgt.BussnessRespositoryModule;

namespace SysMgt.BussnessDomainModule.SysActionType
{
    public class SysActionTypeDomain:IDomainService
    {
        /// <summary>
        /// 申明接口
        /// </summary>
        private IActionTypeRepository actionTypeRepository { get; set; }

        /// <summary>
        /// 构造函数,实例化对象
        /// </summary>
        /// <param name="actionTypeRepository"></param>
        public SysActionTypeDomain(IActionTypeRepository actionTypeRepository)
        {
            this.actionTypeRepository = actionTypeRepository;
        }


        public virtual string Add(SysActionTypeData sysActionTypeData)
        {
            //判断是否存在相同的Code
            var actionTypeInfo = actionTypeRepository.FirstOrDefault(t => t.TYPE_CODE == sysActionTypeData.TypeName);
            if (actionTypeInfo != null)
            {
                return "编码已存在";
            }
            SYS_ACTION_TYPE sysActionType=new SYS_ACTION_TYPE();
            sysActionType.Id = Guid.NewGuid().ToString();
            sysActionType.TYPE_CODE = sysActionTypeData.TypeCode;
            sysActionType.TYPE_NAME = sysActionTypeData.TypeName;
            var returnId = actionTypeRepository.InsertAndGetId(sysActionType);
            if (string.IsNullOrEmpty(returnId))
            {
                return "保存失败！";
            }
            else
            {
                return "保存成功！";
            }
        }

        public virtual string Delete(SysActionTypeData sysActionTypeData)
        {
            actionTypeRepository.Delete(t => t.Id == sysActionTypeData.ID);
            return "删除成功!";
        }

        public virtual string Update(SysActionTypeData sysActionTypeData)
        {
            return "";
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        public virtual PageList<SysActionTypePageResult> GetPageList(SysActionTypeSeachModel search)
        {
            return actionTypeRepository.GetPageList(search);
        }

    }
}
