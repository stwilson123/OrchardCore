using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.SysActionType;
using SysMgt.BussnessDomainModule.SysUserInfo;
using SysMgt.BussnessDTOModule.SysActionType;
using SysMgt.BussnessDTOModule.SysUserInfo;

namespace SysMgt.BussnessApplicationModule
{
    public class SysActionTypeAppService: AppService, ISysActionTypeAppService
    {
        private SysActionTypeDomain sysActionTypeDomain { get; set; }
        public SysActionTypeAppService(SysActionTypeDomain sysActionTypeDomain)
        {
            this.sysActionTypeDomain = sysActionTypeDomain;
        }

        public string Add(SysActionTypeInfo sysActionTypeInfo)
        {
            SysActionTypeData sysActionTypeData=new SysActionTypeData();
            sysActionTypeData.TypeCode = sysActionTypeInfo.TypeCode;
            sysActionTypeData.TypeName = sysActionTypeInfo.TypeName;
            return sysActionTypeDomain.Add(sysActionTypeData);
        }

        public string Delete(SysActionTypeInfo sysActionTypeInfo)
        {
            SysActionTypeData sysActionTypeData = new SysActionTypeData();
            sysActionTypeData.ID = sysActionTypeInfo.ID;
            return sysActionTypeDomain.Delete(sysActionTypeData);
        }

        public string Update(SysActionTypeInfo sysActionTypeInfo)
        {
            SysActionTypeData sysActionTypeData = new SysActionTypeData();
            sysActionTypeData.ID = sysActionTypeInfo.ID;
            sysActionTypeData.TypeCode = sysActionTypeInfo.TypeCode;
            sysActionTypeData.TypeName = sysActionTypeInfo.TypeName;
            return sysActionTypeDomain.Update(sysActionTypeData);
        }
        public PageList<SysActionTypePageResult> GetPageList(SysActionTypeSeachModel search)
        {
            return sysActionTypeDomain.GetPageList(search);
        }

       
    }
}
