using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions.Domain;
using BlocksCore.Domain.Abstractions;
using BlocksCore.Localization.Abtractions; 
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.ThirdSystemType;
using SysMgt.BussnessRespositoryModule.SysLog;
using SysMgt.BussnessRespositoryModule.ThirdSystemType;
using System;
using System.Collections.Generic;

namespace SysMgt.BussnessDomainModule.SysLog
{ 

    public class SysLogDomain : IDomainService
    {
        /// <summary>
        /// 申明接口
        /// </summary>
        private ISysLogRepository SysLogRepository { get; set; }        
       // private IUserContext UserContext;

        /// <summary>
        /// 构造函数,实例化对象
        /// </summary> 
        public SysLogDomain(ISysLogRepository sysLogRepository)
        {
            this.SysLogRepository = sysLogRepository; 
           // this.UserContext = userContext;
        }


    }
}
