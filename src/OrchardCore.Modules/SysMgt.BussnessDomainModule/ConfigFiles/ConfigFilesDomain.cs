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
using SysMgt.BussnessDTOModule.ConfigFiles;
using BlocksCore.Domain.Abstractions;
using SysMgt.BussnessDTOModule.Common;
using SysMgt.BussnessRespositoryModule.ConfigFiles;
using System.IO;//这个引用，文件下载要用到


namespace SysMgt.BussnessDomainModule.ConfigFiles
{
    public class ConfigFilesDomain : IDomainService
    {

        /// <summary>
        /// 申明接口
        /// </summary>
        private IConfigFilesRepository configFilesRepository { get; set; }
        public IStringLocalizer L { get; set; }


        /// <summary>
        /// 构造函数,实例化对象
        /// </summary>
        /// <param name="configFilesRepository"></param>
        public ConfigFilesDomain(IConfigFilesRepository configFilesRepository)
        {
            this.configFilesRepository = configFilesRepository;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        public virtual PageList<ConfigFilesPageResult> GetPageList(ConfigFilesSearchModel search)
        {
            return configFilesRepository.GetPageList(search);
        }

        public virtual string Add(ConfigFilesInfo configFilesInfo)
        {
            BDTA_CONFIGFILES newData = new BDTA_CONFIGFILES();
            newData.Id = Guid.NewGuid().ToString();
            newData.FILE_NAME = configFilesInfo.FileName;
            newData.FILE_PATH = configFilesInfo.FilePath;
            newData.FILE_TYPE = configFilesInfo.FileType;
            newData.FILE_FUNCTION = configFilesInfo.FileFunction;


            var returnId = configFilesRepository.InsertAndGetId(newData);
            if (string.IsNullOrEmpty(returnId))
            {
                throw new BlocksBussnessException("101", L["failed"], null);
            }
            else
            {
                return L["succeed"];
            }
        }

        public virtual string Update(ConfigFilesInfo configFilesInfo)
        {
            int successCount = 0;
            if (configFilesInfo.FileName == "" || configFilesInfo.FilePath == "")
            {
                successCount = configFilesRepository.Update(t => t.Id == configFilesInfo.Id, t => new BDTA_CONFIGFILES()
                {
                    FILE_TYPE = configFilesInfo.FileType,
                    FILE_FUNCTION = configFilesInfo.FileFunction
                });
            }
            else
            {
                successCount = configFilesRepository.Update(t => t.Id == configFilesInfo.Id, t => new BDTA_CONFIGFILES()
                {
                    FILE_TYPE = configFilesInfo.FileType,
                    FILE_NAME = configFilesInfo.FileName,
                    FILE_PATH = configFilesInfo.FilePath,
                    FILE_FUNCTION = configFilesInfo.FileFunction
                });
            }
            if (successCount > 0)
            {
                return L["succeed"];
            }
            else
            {
                throw new BlocksBussnessException("101", L["failed"], null);
            }
        }

        public virtual string Delete(CommonEntity pInfo)
        {
            if (pInfo == null || pInfo.IDs == null || pInfo.IDs.Count <= 0)
            {
                throw new BlocksBussnessException("101", L["请至少选择一笔数据进行删除操作"], null);
            }
            //
            int successCount = configFilesRepository.Update(t => pInfo.IDs.Contains(t.Id), t => new BDTA_CONFIGFILES()
            {                
                ISDELETE = 1
            });

            //long successCount = configFilesRepository.Delete(t => pInfo.IDs.Contains(t.Id));
            if (successCount < pInfo.IDs.Count)
            {
                throw new BlocksBussnessException("101", L["删除失败"], null);
            }
            return "删除成功";
        }

        public virtual ConfigFilesInfo GetOneById(ConfigFilesInfo pInfo)
        {
            var infoData = configFilesRepository.FirstOrDefault(t => t.Id == pInfo.Id);
            if (infoData == null)
            {
                throw new BlocksBussnessException("101", L["查无数据"], null);
            }
            return new ConfigFilesInfo()
            {
                Id = infoData.Id,
                FilePath = infoData.FILE_PATH,
                FileName = infoData.FILE_NAME,
                FileType = infoData.FILE_TYPE,
                FileFunction = infoData.FILE_FUNCTION
            };
        }

        

    }
}
