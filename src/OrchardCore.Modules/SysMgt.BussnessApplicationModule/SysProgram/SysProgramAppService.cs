using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Localization.Abtractions;
using Microsoft.Extensions.Localization;
using SysMgt.BussnessDomainModule.SysProgram;
using SysMgt.BussnessDTOModule.SysProgram;

namespace SysMgt.BussnessApplicationModule.SysProgram
{
	public class SysProgramAppService : AppService, ISysProgramAppService
	{
		public SysProgramDomaim sysProgramDomaim { get; set; }
		public SysProgramAppService(SysProgramDomaim sysProgramDomaim)
		{
			this.sysProgramDomaim = sysProgramDomaim;
		}

		public PageList<SysProgramPageResult> GetPageList(SysProgramSearchModel search)
		{
			return sysProgramDomaim.GetPageList(search);
		}

		[LocalizedDescription("MENU_BINGDING")]
		public void Bind([LocalizedDescription("MENU_BINGDING")]BindSysProgramInfo bindSysProgramInfo)
		{
			BindSysProgramData bindSysProgramData = new BindSysProgramData
			{
				ID = bindSysProgramInfo.ID,
				Platform = bindSysProgramInfo.Platform
			};
			List<SysProgramData> list = new List<SysProgramData>();
			foreach (var item in bindSysProgramInfo.ListsSysProgramInfos)
			{
				list.Add(new SysProgramData()
				{
					ID = item.ID,
					Name = item.Name,
					Sort = item.Sort,
					Code = item.Code
				});
			}

			bindSysProgramData.ListsSysProgramDatas = list;

			sysProgramDomaim.Bind(bindSysProgramData);
		}
	}
}
