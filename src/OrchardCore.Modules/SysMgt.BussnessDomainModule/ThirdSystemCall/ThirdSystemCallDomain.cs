using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions.Domain;
using BlocksCore.Domain.Abstractions;
using BlocksCore.Localization.Abtractions;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.ThirdSystemCall;
using SysMgt.BussnessRespositoryModule.ThirdSystemCall;
using System;

namespace SysMgt.BussnessDomainModule.ThirdSystemCall
{  
    public class ThirdSystemCallDomain : IDomainService
    {
        /// <summary>
        /// 申明接口
        /// </summary>
        private IThirdSystemCallRepository ThirdSystemCallRepository { get; set; }
        private IThirdSystemCallLogRepository ThirdSystemCallLogRepository { get; set; }

        public Localizer L { get; set; }
       // private long MaxRequestTimes = 3;//每个接口最多请求调用次数

        /// <summary>
        /// 构造函数,实例化对象
        /// </summary> 
        public ThirdSystemCallDomain(IThirdSystemCallRepository thirdSystemCallRepository, IThirdSystemCallLogRepository thirdSystemCallLogRepository)
        {
            this.ThirdSystemCallRepository = thirdSystemCallRepository;
            this.ThirdSystemCallLogRepository = thirdSystemCallLogRepository;           
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        public virtual PageList<ThirdSystemCallPageResult> GetPageList(ThirdSystemCallSearchModel search)
        {
            return ThirdSystemCallRepository.GetPageList(search);
        }

        /// <summary>
        /// 根据ID获取单笔数据
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public virtual ThirdSystemCallInfo GetOneById(ThirdSystemCallInfo pInfo)
        {
            var infoData = ThirdSystemCallRepository.FirstOrDefault(t => t.Id == pInfo.ID);
            if (infoData == null)
            {
                throw new BlocksBussnessException("101", L("查无数据"), null);
            }
            return new ThirdSystemCallInfo()
            {
                ID = infoData.Id,
                SystemID=infoData.SYSTEM_ID,
                SystemNo = infoData.SYSTEM_NO,
                SystemName = infoData.SYSTEM_NAME,
                FunctionName=infoData.FUNCTION_NAME,
                ProcessTimeBegin=infoData.PROCESS_TIME_BEGIN,
                ProcessTimeEnd=infoData.PROCESS_TIME_END,
                ProcessResult=infoData.PROCESS_RESULT,
                ParameterIn=infoData.PARAMETER_IN,
                RequestTimes=infoData.REQUEST_TIMES,
                ResponseValue=infoData.RESPONSE_VALUE,
                ExceptionMsg=infoData.EXCEPTION_MSG 
            };
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public virtual string Update(ThirdSystemCallInfo pInfo)
        {

            //判断是否存在相同系统类型编号
            var infoData = ThirdSystemCallRepository.FirstOrDefault(t => t.Id == pInfo.ID);
            if (infoData == null)
            {
                throw new BlocksBussnessException("101", L("查无数据"), null);
            }
            if (infoData.PROCESS_RESULT == 2) //0-新建；1-请求中；2-成功；3-失败（调用成功单结果失败和调用失败）
            {
                throw new BlocksBussnessException("101", L("接口已成功调用了，不允许更改传入参数"), null);
            }
            if (infoData.PROCESS_RESULT == 1)
            {
                throw new BlocksBussnessException("101", L("接口调用尚在请求中，不允许更改传入参数"), null);
            }
            //if (infoData.REQUEST_TIMES >= MaxRequestTimes)
            //{
            //    throw new BlocksBussnessException("101", L("接口已超过最大允许调用次数{0}了，不允许更改传入参数", MaxRequestTimes), null);
            //}
            int successCount = ThirdSystemCallRepository.Update(t => t.Id == pInfo.ID && (t.PROCESS_RESULT == 3  || t.PROCESS_RESULT == 0), t => new SYS_CALL_THIRD_SYSTEM_INFO()
            {
                PARAMETER_IN = pInfo.ParameterIn,
                DATAVERSION = t.DATAVERSION + 1
            });
            if (successCount <= 0)
            {
                throw new BlocksBussnessException("101", L("更新失败（数据不存在或处理结果不是失败状态）"), null);
            }
            return "更新成功";
        }

        /// <summary>
        /// 发起接口重传申请
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public virtual string ReCall(CommonEntity pInfo)
        {
            
            if (pInfo == null || pInfo.IDs == null || pInfo.IDs.Count <= 0)
            {
                throw new BlocksBussnessException("101", L("请选择一笔数据进行重传操作"), null);
            }

            var infoData = ThirdSystemCallRepository.GetAllList(t => pInfo.IDs.Contains(t.Id));
            if (infoData == null || infoData.Count <= 0)
            {
                throw new BlocksBussnessException("101", L("查无数据"), null);
            }
            foreach (var item in infoData)
            {
                if (item.PROCESS_RESULT == 2)//0-新建；1-请求中；2-成功；3-失败（调用成功单结果失败和调用失败）
                {
                    throw new BlocksBussnessException("101", L("接口已成功调用了,不允许重传"), null);
                }
                if (item.PROCESS_RESULT == 1)
                {
                    throw new BlocksBussnessException("101", L("接口调用尚在请求中，请耐心等待"), null);
                }
                int successCount = ThirdSystemCallRepository.Update(t => t.Id == item.Id && t.PROCESS_RESULT == 3, t => new SYS_CALL_THIRD_SYSTEM_INFO()
                {
                    REQUEST_TIMES = 0,
                    PROCESS_RESULT = 0,
                    PROCESS_TIME_BEGIN=DateTime.Now,
                    RESPONSE_VALUE=null,
                    EXCEPTION_MSG=null,
                    DATAVERSION = t.DATAVERSION + 1
                });
                if (successCount <= 0)
                {
                    throw new BlocksBussnessException("101", L("重传申请失败（数据不存在或处理结果不是失败状态）"), null);
                }
            }
            return "重传申请成功";
        }
    }
}
