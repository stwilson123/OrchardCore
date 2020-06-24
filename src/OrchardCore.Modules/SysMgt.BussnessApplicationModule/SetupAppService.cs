using BlocksCore.Application.Abstratctions;
using BlocksCore.AutoMapper.Abstractions;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.Setup;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessDTOModule.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessApplicationModule
{
    public class SetupAppService : IAppService, ISetupAppService
    {
        private SetupDomain setupDomain { get; set; }
        public SetupAppService(SetupDomain setupDomain)
        {
            this.setupDomain = setupDomain;
        }

        #region 系统配置类型相关服务
        public PageList<SetupTypePageResult> GetSetupTypePageList(SetupTypeSearchModel search)
        {
            return setupDomain.GetSetupTypePageList(search);
        }

        public string AddSetupTypeAndDetail(SetupTypeInfo setupTypeInfo)
        {          
            return setupDomain.AddSetupTypeAndDetail(setupTypeInfo);
        }

        public SetupTypeInfo GetSetupTypeById(SetupTypeInfo setupInfo)
        {            
            return setupDomain.GetSetupTypeById(setupInfo);
        }

        public string EditSetupTypeAndDetail(SetupTypeInfo setupTypeInfo)
        {
            return setupDomain.EditSetupTypeAndDetail(setupTypeInfo);
        }

        public  string DeleteSetupTypeById(CommonEntity setupTypeId)
        {
            return setupDomain.DeleteSetupTypeById(setupTypeId.IDs);
        }
        #endregion

        #region 系统配置内容相关服务
        public string Add(SetupInfo setupInfo)
        {
            SetupData setupDomainModel4Opt = new SetupData();
            setupDomainModel4Opt.SetupNo = setupInfo.SetupNo;
            setupDomainModel4Opt.SetupContents = setupInfo.SetupContents;
            setupDomainModel4Opt.SetupParameter = setupInfo.SetupParameter;

            return setupDomain.Add(setupDomainModel4Opt);
        }

        public PageList<SetupPageResult> GetPageList(SetupSearchModel search)
        {
            return setupDomain.GetPageList(search);
        }

        public string Update(SetupInfo setupInfo)
        {
            SetupData setupData = new SetupData();
            setupData.ID = setupInfo.ID;
            setupData.SetupNo = setupInfo.SetupNo;
            setupData.SetupContents = setupInfo.SetupContents;
            setupData.SetupParameter = setupInfo.SetupParameter;
            return setupDomain.Update(setupData);
        }

        public string Delete(CommonEntity entity4Delete)
        {
            return setupDomain.Delete(entity4Delete.IDs);
        }

        public SetupData GetOneById(SetupInfo setupInfo)
        {
            SetupData setupData = setupInfo.AutoMapTo<SetupData>();
            return setupDomain.GetOneById(setupData);
        }

        public SetupData GetOneByCode(SetupInfo setupInfo)
        {
            SetupData setupData = setupInfo.AutoMapTo<SetupData>();
            return setupDomain.GetOneByCode(setupData);
        }

        public PageList<ComboboxData> GetComboxListByPrinter(SearchModel search)
        {
            return setupDomain.GetComboxListByPrinter(search);
        }
        #endregion
    }
}
