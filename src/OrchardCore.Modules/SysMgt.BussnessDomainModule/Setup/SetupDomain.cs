using BlocksCore.Domain.Abstractions.Domain;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions;
using BlocksCore.Localization.Abtractions;
using SysMgt.BussnessDomainModule.Common;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.Setup;
using SysMgt.BussnessRespositoryModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysMgt.BussnessDomainModule.SysGlobal;


namespace SysMgt.BussnessDomainModule.Setup
{
    public class SetupDomain : IDomainService
    {
     //   public SysGlobalDomain SysGlobalDomain { get; set; }
        /// <summary>
        /// 申明接口
        /// </summary>
        private ISetupRepository setupRepository { get; set; }
        private ISetupTypeRepository setupTypeRepository { get; set; }

        public Localizer L { get; set; }
        /// <summary>
        /// 构造函数,实例化对象
        /// </summary>
        /// <param name="departmentRepository"></param>
        public SetupDomain(ISetupRepository setupRepository, ISetupTypeRepository setupTypeRepository)
        {
            this.setupRepository = setupRepository;
            this.setupTypeRepository = setupTypeRepository;
        }

        #region 系统配置类型相关业务逻辑
        public virtual PageList<SetupTypePageResult> GetSetupTypePageList(SetupTypeSearchModel search)
        {
            return setupTypeRepository.GetPageList(search);
        }

        public string AddSetupTypeAndDetail(SetupTypeInfo setupTypeInfo)
        {
            #region 校验
            if (string.IsNullOrEmpty(setupTypeInfo.SetupTypeNo))
            {
                throw new BlocksBussnessException("101", L("系统配置类型编码不能为空！"), null);
            }
            var curEntity = setupTypeRepository.FirstOrDefault(t => t.SETUP_TYPE_NO == setupTypeInfo.SetupTypeNo);
            if (curEntity != null)
            {
                throw new BlocksBussnessException("101", L("系统配置类型编码已存在！"), null);
            }
            if (setupTypeInfo.SetupList != null || setupTypeInfo.SetupList.Count > 0)
            {
                var setupNos = setupTypeInfo.SetupList.Select(x => x.SetupNo).Distinct().ToList();
                if (setupNos.Count() < setupTypeInfo.SetupList.Count())
                {
                    throw new BlocksBussnessException("101", L("系统配置明细数据中的编码不能重复！"), null);
                }
            }
            #endregion

            #region 解析json数据，并且赋值对象
            BDTA_SETUP_TYPE model = new BDTA_SETUP_TYPE();
            string setupTypeID = Guid.NewGuid().ToString();
            model.Id = setupTypeID;
            model.SETUP_TYPE_NO = setupTypeInfo.SetupTypeNo;
            model.SETUP_TYPE_NAME = setupTypeInfo.SetupTypeName;
            model.SETUP_TYPE_VALUE = setupTypeInfo.SetupTypeValue;
            model.ACTIVITY = 1;
            model.ISUSED = 0;

            List<BDTA_SETUP> setupList = new List<BDTA_SETUP>();
            foreach (var item in setupTypeInfo.SetupList)
            {
                BDTA_SETUP setup = new BDTA_SETUP();
                setup.Id = Guid.NewGuid().ToString();
                setup.SETUP_NO = item.SetupNo;
                setup.SETUP_PARAMETER = item.SetupParameter;
                setup.SETUP_CONTENTS = item.SetupContents;
                setup.SETUP_TYPE = setupTypeInfo.SetupTypeNo;
                setup.SETUP_TYPE_ID = setupTypeID;
                setup.ISUSED = 0;
                setup.ACTIVITY = 1;
                setupList.Add(setup);
            }
            #endregion

            #region 新增
            string isSuccessed = setupTypeRepository.InsertAndGetId(model);
            if (string.IsNullOrEmpty(isSuccessed))
            {
                throw new BlocksBussnessException("101", L("系统配置类型添加失败！"), null);
            }
            if (setupList.Count > 0)
            {
                var sucFlag = setupRepository.Insert(setupList);
                if (sucFlag.Count <= 0)
                {
                    throw new BlocksBussnessException("101", L("系统配置明细数据添加失败！"), null);
                }
            }

            //增加外挂后续保存数据适用于新增和更新  朱煜轲 自定义字段 先注释掉
            //SysGlobalData sysGlobalData = new SysGlobalData();
            //sysGlobalData.OperateType = "INSERT";
            //sysGlobalData.Cid = setupTypeID;
            //sysGlobalData.CKey = "1";
            //sysGlobalData.ValueList = setupTypeInfo.CodeList.AutoMapTo<List<KeyValue>>();
            //SysGlobalDomain.OperateCustomerSQL(sysGlobalData);
            return "保存成功！";
            #endregion
        }

        public virtual SetupTypeInfo GetSetupTypeById(SetupTypeInfo setupTypeData)
        {
            var model = setupTypeRepository.FirstOrDefault(t => t.Id == setupTypeData.ID);
            if (model == null)
            {
                throw new BlocksBussnessException("101", L("未查到系统配置类型编码数据！"), null);
            }
            var list = setupRepository.GetAllList(x => x.SETUP_TYPE_ID == setupTypeData.ID);
            List<SetupInfo> setupList = new List<SetupInfo>();
            if (list.Count > 0)
            {
                setupList = list.Select(x => new SetupInfo()
                {
                    ID=x.Id,
                    SetupNo=x.SETUP_NO,
                    SetupContents=x.SETUP_CONTENTS,
                    SetupParameter=x.SETUP_PARAMETER                    
                }).ToList();
            }
            return new SetupTypeInfo()
            {
                ID = model.Id,
                SetupTypeNo = model.SETUP_TYPE_NO,
                SetupTypeName = model.SETUP_TYPE_NAME,
                SetupTypeValue = model.SETUP_TYPE_VALUE,
                SetupList=setupList
                
            };
        }

        public string EditSetupTypeAndDetail(SetupTypeInfo setupTypeInfo)
        {
            #region 校验
            if (string.IsNullOrEmpty(setupTypeInfo.ID))
            {
                throw new BlocksBussnessException("101", L("系统配置类型ID不能为空！"), null);
            }
            if (string.IsNullOrEmpty(setupTypeInfo.SetupTypeNo))
            {
                throw new BlocksBussnessException("101", L("系统配置类型编码不能为空！"), null);
            }
            var curEntity = setupTypeRepository.FirstOrDefault(t => t.SETUP_TYPE_NO == setupTypeInfo.SetupTypeNo && t.Id!= setupTypeInfo.ID);
            if (curEntity != null)
            {
                throw new BlocksBussnessException("101", L("系统配置类型编码已存在！"), null);
            }
            if (setupTypeInfo.SetupList != null || setupTypeInfo.SetupList.Count > 0)
            {
                var setupNos = setupTypeInfo.SetupList.Select(x => x.SetupNo).Distinct().ToList();
                if (setupNos.Count() < setupTypeInfo.SetupList.Count())
                {
                    throw new BlocksBussnessException("101", L("系统配置明细数据中的编码不能重复！"), null);
                }
            }
            #endregion

            #region 解析json数据，并且赋值对象
            List<BDTA_SETUP> setupList = new List<BDTA_SETUP>();
            foreach (var item in setupTypeInfo.SetupList)
            {
                BDTA_SETUP setup = new BDTA_SETUP();
                Guid newid = Guid.NewGuid();                
                setup.Id = Guid.TryParse(item.ID, out newid)?item.ID : Guid.NewGuid().ToString();
                setup.SETUP_NO = item.SetupNo;
                setup.SETUP_PARAMETER = item.SetupParameter;
                setup.SETUP_CONTENTS = item.SetupContents;
                setup.SETUP_TYPE = setupTypeInfo.SetupTypeNo;
                setup.SETUP_TYPE_ID = setupTypeInfo.ID;
                setup.ACTIVITY = 1;
                setup.ISUSED = 0;
                setupList.Add(setup);
            }
            #endregion

            #region 编辑
            
            int isSuccessed = setupTypeRepository.Update(x=>x.Id== setupTypeInfo.ID, x=>new BDTA_SETUP_TYPE() {
                SETUP_TYPE_NO= setupTypeInfo.SetupTypeNo,
                SETUP_TYPE_NAME= setupTypeInfo.SetupTypeName,
                SETUP_TYPE_VALUE= setupTypeInfo.SetupTypeValue
            });            
            if (isSuccessed<=0)
            {
                throw new BlocksBussnessException("101", L("系统配置类型更新失败！"), null);
            }
            long delFlag = setupRepository.Delete(x => x.SETUP_TYPE_ID == setupTypeInfo.ID);
            if (setupList.Count > 0)
            {
                var sucFlag = setupRepository.Insert(setupList);
                if (sucFlag.Count <= 0)
                {
                    throw new BlocksBussnessException("101", L("系统配置明细数据添加失败！"), null);
                }
            }
            return "保存成功！";
            #endregion
        }

        public virtual string DeleteSetupTypeById(List<string> Ids)
        {
            //if (Ids == null || Ids.Count <= 0)
            //{
            //    HelperBLL.ThrowEx("101", L("请选择一条系统配置类型数据删除！"));
            //}
            foreach (var setupTypeId in Ids)
            {
                var delType = setupTypeRepository.Delete(x => x.Id == setupTypeId && x.ISUSED == 0);
                if (delType <= 0)
                {
                    throw new BlocksBussnessException("101", L("系统配置类型删除失败！"), null);
                }
                var delinfo = setupRepository.Delete(x => x.SETUP_TYPE_ID == setupTypeId && x.ISUSED == 0);
            }
            return "删除成功！";
        }

        #endregion

        #region 系统配置内容相关业务逻辑
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        public virtual PageList<SetupPageResult> GetPageList(SetupSearchModel search)
        {
            return setupRepository.GetPageList(search);
        }
        
        public  string Add(SetupData setupData)
        {
            var curEntity = setupRepository.FirstOrDefault(t => t.SETUP_NO == setupData.SetupNo);
            if (curEntity != null)
            {
                throw new BlocksBussnessException("101", L("编码已存在！"), null);

            }
            if (setupData.SetupNo == "")
            {
                throw new BlocksBussnessException("101", L("编码不能为空！"), null);
            }
            #region 解析json数据，并且赋值对象

            BDTA_SETUP model = new BDTA_SETUP();
            model.Id = Guid.NewGuid().ToString();
            model.SETUP_NO = setupData.SetupNo;
            model.SETUP_CONTENTS = setupData.SetupContents;
            model.SETUP_PARAMETER = setupData.SetupParameter;
            model.SETUP_KEY = "1";//暂定测试
            #endregion

            #region 新增
            string isSuccessed = setupRepository.InsertAndGetId(model);
            if (string.IsNullOrEmpty(isSuccessed))
            {
                //throw new BlocksBussnessException("101", L("保存失败！"), null);
                return "保存失败";
            }
            else
            { return "保存成功！"; }
          

            #endregion

        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="outInTypeData"></param>
        /// <returns></returns>
        public virtual string Update(SetupData setupData)
        {
           

            #region 判断主目录是否已存在

            var curEntity = setupRepository.FirstOrDefault(t => t.SETUP_NO == setupData.SetupNo && t.Id != setupData.ID);
            if (curEntity != null)
            {
                throw new BlocksBussnessException("101", L("编号已存在！"), null);
            }

            #endregion

            #region 编辑

            int successCount = setupRepository.Update(t => t.Id == setupData.ID, t => new BDTA_SETUP()
            {
                SETUP_NO = setupData.SetupNo,
                SETUP_CONTENTS = setupData.SetupContents,
                SETUP_PARAMETER = setupData.SetupParameter,

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

            //var outInType = OutInRepository.GetAllList(t => ids.Contains(t.Id));

            //foreach (string id in ids)
            //{
            //    int successCount = OutInRepository.Update(t => t.Id == id, t => new BDTA_OUT_IN_TYPE()
            //    {
            //        ACTIVITY = 0,
            //        OUT_IN_NO = t.OUT_IN_NO + "-" + t.Id,

            //    });
            //    if (successCount == 0)
            //    {
            //        HelperBLL.ThrowEx("101", "更新失败！");
            //    }
            //}

           setupRepository.Delete(t => ids.Contains(t.Id) && t.ISUSED == 0);


            return "删除成功！";

        }

        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="setupData"></param>
        /// <returns></returns>
        public virtual SetupData GetOneById(SetupData setupData)
        {
            var model = setupRepository.FirstOrDefault(t => t.Id == setupData.ID);
            if (model == null)
            {
                throw new BlocksBussnessException("101", L("未查到对象！"), null);        
            }

            return new SetupData()
            {

                ID = model.Id,
                SetupNo = model.SETUP_NO,
                SetupContents = model.SETUP_CONTENTS,
                SetupParameter = model.SETUP_PARAMETER

            };

        }

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="setupData"></param>
        /// <returns></returns>
        public virtual SetupData GetOneByCode(SetupData setupData)
        {  
            var model = setupRepository.FirstOrDefault(t => t.SETUP_NO == setupData.SetupNo);
            if (model == null)
            {
                throw new BlocksBussnessException("101", L("未查到对象！"), null);
            }

            return new SetupData()
            {

                ID = model.Id,
                SetupNo = model.SETUP_NO,
                SetupContents = model.SETUP_CONTENTS,
                SetupParameter = model.SETUP_PARAMETER

            };

        }

        public virtual PageList<ComboboxData> GetComboxListByPrinter(SearchModel search)
        {
            string type = "Printer";
            return setupRepository.GetComboxListByType(search,type);
        }
        #endregion
    }
}
