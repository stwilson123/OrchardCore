//using Blocks.BussnessEntityModule;

//using Blocks.Framework.Event;
//using Microsoft.Extensions.Localization;
//using Microsoft.Extensions.Logging;
//using Blocks.Framework.Navigation;
//using Blocks.Framework.Navigation.Event;
//using BlocksCore.Abstractions.Security;
//using Blocks.Framework.Web.Navigation;
//using SysMgt.BussnessRespositoryModule;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SysMgt.BussnessDomainModule.SysProgram
//{
//	public class SysProgramInitEventHandler : IDomainEventHandler<MenusInitEventData>
//	{

//		public ISysProgramRepository SysProgramRepository { get; set; }
//		public IMenuRepository MenuRepository { get; set; }
//		private IUserContext UserContext { get; set; }

//		public ILog log { get; set; }
//		public SysProgramInitEventHandler(ISysProgramRepository SysProgramRepository, IMenuRepository MenuRepository, IUserContext UserContext)
//		{
//			this.SysProgramRepository = SysProgramRepository;
//			this.MenuRepository = MenuRepository;
//			this.UserContext = UserContext;

//		}

//		public void HandleEvent(MenusInitEventData eventData)
//		{
//			var navs = eventData.NavigationItems;
//			var pcitems = navs["MainMenu"];
//			var mobileitems = navs["Mobile"];
//			var sysPrograms = SysProgramRepository.GetAllList();
//			List<SYS_PROGRAM> sysProgramList = new List<SYS_PROGRAM>();
//			foreach (var item in pcitems.Cast<WebNavigationItemDefinition>())
//			{
//				if (string.IsNullOrEmpty(item.Url))
//				{
//					return;
//				}
//				Stopwatch sw = Stopwatch.StartNew();
//				//查询数据库中是否存在改菜单，如果存在更新信息，否则新增
//				var sysProgramInfo = sysPrograms.FirstOrDefault(t => t.PROGRAMCODE == item.GetUniqueId());
//				if (sysProgramInfo == null)
//				{
//					SYS_PROGRAM sysProgram = new SYS_PROGRAM();
//					sysProgram.Id = Guid.NewGuid().ToString();
//					sysProgram.PROGRAMCODE = item.GetUniqueId();

//					sw.Restart();
//					sysProgram.PROGRAMNAME = item.DisplayName;
//					sw.Stop();
//					Trace.TraceInformation($"automapto cost time {sw.ElapsedMilliseconds}ms");
//					sysProgram.PROGRAMPROPERTY = item.Url;
//					sysProgram.ACTIVITY = 0;
//					sysProgram.CREATER = " ";
//					sysProgram.UPDATER = " ";
//					sysProgram.PLATFORM = 1;
//					sysProgramList.Add(sysProgram);
//				}
//				else if (sysProgramInfo.PROGRAMCODE != item.GetUniqueId() || sysProgramInfo.PROGRAMNAME != item.DisplayName.ToString()
//					|| sysProgramInfo.PROGRAMPROPERTY != item.Url)
//				{
//					//SysProgramRepository.Update(t => t.Id == sysProgramInfo.Id, t => new SYS_PROGRAM()
//					//{
//					//    PROGRAMCODE = item.GetUniqueId(),
//					//    PROGRAMNAME = item.DisplayName.AutoMapTo<string>(),
//					//    PROGRAMPROPERTY = item.Url,
//					//    UPDATER= " "
//					//});
//				}
//			}
//			foreach (var item in mobileitems.Cast<WebNavigationItemDefinition>())
//			{
//				Stopwatch sw = Stopwatch.StartNew();
//				var sysProgramInfo = sysPrograms.FirstOrDefault(t => t.PROGRAMCODE == item.GetUniqueId());
//				if (sysProgramInfo == null)
//				{
//					SYS_PROGRAM sysProgram = new SYS_PROGRAM();
//					sysProgram.Id = Guid.NewGuid().ToString();
//					sysProgram.PROGRAMCODE = item.GetUniqueId();
//					sw.Restart();
//					sysProgram.PROGRAMNAME = item.DisplayName;
//					sw.Stop();
//					Trace.TraceInformation($"automapto cost time {sw.ElapsedMilliseconds}ms");
//					sysProgram.PROGRAMPROPERTY = "";
//					sysProgram.ACTIVITY = 0;
//					sysProgram.CREATER = " ";
//					sysProgram.UPDATER = " ";
//					sysProgram.PLATFORM = 2;
//					sysProgramList.Add(sysProgram);
//				}
//			}

//			if (sysProgramList.Count > 0)
//			{
//				SysProgramRepository.Insert(sysProgramList);
//			}
//		}
//	}
//}