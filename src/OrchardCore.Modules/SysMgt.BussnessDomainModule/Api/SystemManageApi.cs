using Blocks.BussnessEntityModule;

using BlocksCore.Domain.Abstractions.Domain;
using BlocksCore.Domain.Abstractions;
using BlocksCore.Localization.Abtractions;
using Microsoft.Extensions.Logging;
using BlocksCore.Abstractions.Security;
using Newtonsoft.Json;
using SysMgt.BussnessDomainModule.SysLog;
using SysMgt.BussnessDomainModule.ThirdSystemCall;
using SysMgt.BussnessRespositoryModule.SysLog;
using SysMgt.BussnessRespositoryModule.ThirdSystemCall;
using SysMgt.BussnessRespositoryModule.ThirdSystemType;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SysMgt.BussnessDomainModule.Api
{
    public class SystemManageApi : ISystemManageApi
    {
        public Localizer L { get; set; }
        private ILogger Ilog { get; set; }
        private IUserContext userContext { get; set; }
        private IThirdSystemCallRepository thirdSystemCallRepository { get; set; }
        private IThirdSystemTypeRepository thirdSystemTypeRepository { get; set; }
        private ISysLogRepository SysLogRepository { get; set; }
        public SystemManageApi(IThirdSystemCallRepository thirdSystemCallRepository, IThirdSystemTypeRepository thirdSystemTypeRepository,
            ISysLogRepository sysLogRepository, ILogger<SystemManageApi> Ilog, IUserContext userContext)
        {
            this.thirdSystemCallRepository = thirdSystemCallRepository;
            this.thirdSystemTypeRepository = thirdSystemTypeRepository;
            this.SysLogRepository = sysLogRepository;
            this.Ilog = Ilog;
            this.userContext = userContext;

        }

        public string ThirdSystemCallApply(ThirdSystemCallData pInfo)
        {
            #region 入参校验
            if (pInfo == null)
            {
                throw new BlocksBussnessException("101", L("传入参数为空"), null);
            }
            if (string.IsNullOrEmpty(pInfo.SystemNo))
            {
                throw new BlocksBussnessException("101", L("系统编号参数SystemNo为空"), null);
            }
            var systemType = thirdSystemTypeRepository.FirstOrDefault(t => t.SYSTEM_NO == pInfo.SystemNo);
            if (systemType == null)
            {
                throw new BlocksBussnessException("101", L("系统编号在系统类型中未维护，请先维护系统类型"), null);
            }
            if (string.IsNullOrEmpty(pInfo.FunctionName))
            {
                throw new BlocksBussnessException("101", L("方法名称参数FunctionName为空"), null);
            }
            if (!string.IsNullOrEmpty(pInfo.ParameterIn))
            {
                try
                {
                    var parameterIn = JsonConvert.DeserializeObject(pInfo.ParameterIn);
                }
                catch
                {
                    throw new BlocksBussnessException("101", L("方法名称参数ParameterIn不是正确的JSON字符串格式"), null);
                }
            }
            #endregion

            #region 数据准备
            SYS_CALL_THIRD_SYSTEM_INFO applyData = new SYS_CALL_THIRD_SYSTEM_INFO
            {
                Id = Guid.NewGuid().ToString(),
                SYSTEM_ID = systemType.Id,
                SYSTEM_NO = pInfo.SystemNo,
                SYSTEM_NAME = pInfo.SystemName,
                FUNCTION_NAME = pInfo.FunctionName,
                PARAMETER_IN = pInfo.ParameterIn,
                PROCESS_TIME_BEGIN = DateTime.Now,
                REQUEST_TIMES = 0,
                PROCESS_RESULT = 0,
                DATAVERSION = 1
            };
            #endregion

            #region 数据库操作
            string newId = thirdSystemCallRepository.InsertAndGetId(applyData);
            if (string.IsNullOrEmpty(newId))
            {
                throw new BlocksBussnessException("101", L("接口调用申请失败"), null);
            }
            #endregion

            return L("succeed");
        }
        /// <summary>
        /// 上架单新增方法，新增数据包括基础主信息及明细信息，上架单号请于前置逻辑中调用通用编码生成逻辑进行生成，或自行编写生成逻辑生成。
        /// 上架单号不能为空，上架单号重复性验证，物料、批次合法性验证，物料、批次重复性验证
        /// 本方法只进行数据库交互活动
        /// </summary>
        /// <param name="SysLogData">系统日志模型</param>
        /// <returns name="RtnData">1-成功</returns>
        public RtnData WriteSystemLog(SysLogData sysLogData)
        {
            
            SYS_LOG sysLog = new SYS_LOG();
            sysLog.LOGID = Guid.NewGuid().ToString();
            sysLog.MODULENAME = sysLogData.MethodName;
            sysLog.CLASSNAME = sysLogData.ClassName;
            sysLog.METHODNAME = sysLogData.MethodName;
            sysLog.USERID = userContext.GetCurrentUser().UserId;
            sysLog.TAKETIME = (decimal)sysLogData.TakeTime.TotalMilliseconds;
            sysLog.LOGLEVEL = sysLogData.LogLevel;
            sysLog.LOGDATE = DateTime.Now;
            sysLog.LOGEXCEPTION = sysLogData.LogException;
            //sysLog.COMPUTERNAME = userContext;
            //sysLog.IPADDRESS = sysLogData.IPAddress;
            sysLog.CUSTOMMESSAGE = sysLogData.CustomMessage;
            SysLogRepository.Insert(sysLog);

        
            Ilog.LogInformation(String.Format(sysLog.MODULENAME + "/" + sysLog.CLASSNAME + "/" + sysLog.METHODNAME + " take up time " + sysLog.TAKETIME.ToString()));

            RtnData RtnData = new RtnData();
            RtnData.RtnCode = "1";
            RtnData.RtnNote = L("succeed");
            return RtnData;
        }

    }
}
