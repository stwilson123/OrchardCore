using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions.Domain;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.Localization; 
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.ThirdSystemType;
using SysMgt.BussnessRespositoryModule.ThirdSystemType;
using System;
using System.Collections.Generic;

namespace SysMgt.BussnessDomainModule.ThirdSystemType
{ 

    public class ThirdSystemTypeDomain : IDomainService
    {
        /// <summary>
        /// 申明接口
        /// </summary>
        private IThirdSystemTypeRepository ThirdSystemTypeRepository { get; set; }        
       // private IUserContext UserContext;
       public IStringLocalizer L { get; set; }
        /// <summary>
        /// 构造函数,实例化对象
        /// </summary> 
        public ThirdSystemTypeDomain(IThirdSystemTypeRepository thirdSystemTypeRepository)
        {
            this.ThirdSystemTypeRepository = thirdSystemTypeRepository; 
           // this.UserContext = userContext;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        public virtual PageList<ThirdSystemTypePageResult> GetPageList(ThirdSystemTypeSearchModel search)
        {
            return ThirdSystemTypeRepository.GetPageList(search);
        }

        /// <summary>
        /// 根据ID获取单笔数据
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public virtual ThirdSystemTypeInfo GetOneById(ThirdSystemTypeInfo pInfo)
        {
            var infoData = ThirdSystemTypeRepository.FirstOrDefault(t => t.Id == pInfo.ID);
            if (infoData == null)
            {
                throw new BlocksBussnessException("101", L["查无数据"], null);
            }
            return new ThirdSystemTypeInfo()
            {
                ID = infoData.Id,
                SystemNo=infoData.SYSTEM_NO,
                SystemName=infoData.SYSTEM_NAME
            };
        }
   
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public virtual string Add(ThirdSystemTypeInfo pInfo)
        {
            if (string.IsNullOrEmpty(pInfo.SystemNo))
            {
                throw new BlocksBussnessException("101", L["请填写系统编号"], null);
            }
            if (string.IsNullOrEmpty(pInfo.SystemName))
            {
                throw new BlocksBussnessException("101", L["请填写系统名称"], null);
            }
            //判断是否存在相同系统类型编号
            var infoData = ThirdSystemTypeRepository.FirstOrDefault(t => t.SYSTEM_NO == pInfo.SystemNo);
            if (infoData != null)
            {
                throw new BlocksBussnessException("101", L["系统编号已存在"], null); 
            }

            SYS_THIRD_SYSTEM_TYPE newData = new SYS_THIRD_SYSTEM_TYPE();
            newData.Id = Guid.NewGuid().ToString();
            newData.SYSTEM_NO = pInfo.SystemNo;
            newData.SYSTEM_NAME = pInfo.SystemName;
            newData.DATAVERSION = 1;
            newData.ACTIVITY = 1;
            newData.ISUSED = 0;
           
            var returnId = ThirdSystemTypeRepository.InsertAndGetId(newData);
            if (string.IsNullOrEmpty(returnId))
            {
                throw new BlocksBussnessException("101", L["新增失败"], null);
            }
            
             return "新增成功";             
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public virtual string Update(ThirdSystemTypeInfo pInfo)
        {
            if (string.IsNullOrEmpty(pInfo.SystemNo))
            {
                throw new BlocksBussnessException("101", L["请填写系统编号"], null);
            }
            if (string.IsNullOrEmpty(pInfo.SystemName))
            {
                throw new BlocksBussnessException("101", L["请填写系统名称"], null);
            }
            //判断是否存在相同系统类型编号
            var infoData = ThirdSystemTypeRepository.FirstOrDefault(t => t.SYSTEM_NO == pInfo.SystemNo && t.Id != pInfo.ID);
            if (infoData != null)
            {
                throw new BlocksBussnessException("101", L["系统编号已存在"], null);
            }
            int successCount = ThirdSystemTypeRepository.Update(t => t.Id == pInfo.ID, t => new SYS_THIRD_SYSTEM_TYPE()
            {
                SYSTEM_NO = pInfo.SystemNo,
                SYSTEM_NAME = pInfo.SystemName,
                DATAVERSION = t.DATAVERSION + 1
            });
            if (successCount <= 0)
            {
                throw new BlocksBussnessException("101", L["更新失败"], null);
            }
            return "更新成功";
        }

        /// <summary>
        /// 删除（支持批量）
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public virtual string Delete(CommonEntity pInfo)
        {
            if (pInfo == null || pInfo.IDs == null || pInfo.IDs.Count <= 0)
            {
                throw new BlocksBussnessException("101", L["请至少选择一笔数据进行删除操作"], null);
            } 

            long successCount = ThirdSystemTypeRepository.Delete(t => pInfo.IDs.Contains(t.Id));
            if (successCount < pInfo.IDs.Count)
            {
                throw new BlocksBussnessException("101", L["删除失败"], null);
            }
            return "删除成功";
        }

        public virtual List<ComboboxData> GetComboxList()
        {
            var list= ThirdSystemTypeRepository.GetAllList();
            List<ComboboxData> rtnData = new List<ComboboxData>();
            foreach (var item in list)
            {
                rtnData.Add(new ComboboxData
                {
                    Id=item.Id,
                    Text=item.SYSTEM_NAME
                });
            }
            return rtnData;


        }

    }
}
