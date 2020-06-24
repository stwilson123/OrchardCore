using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDTOModule.SysActionType;

namespace SysMgt.BussnessApplicationModule
{
   public interface ISysActionTypeAppService: IAppService
   {

       string Add(SysActionTypeInfo sysActionTypeInfo);

       string Delete(SysActionTypeInfo sysActionTypeInfo);
       string Update(SysActionTypeInfo sysActionTypeInfo);

       PageList<SysActionTypePageResult> GetPageList(SysActionTypeSeachModel search);
   }
}
