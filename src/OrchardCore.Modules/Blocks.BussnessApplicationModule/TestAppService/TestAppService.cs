using System;
using System.Collections.Generic;
using System.Linq;
using Blocks.BussnessDomainModule;
using Blocks.BussnessDomainModule.MasterData;
using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.MasterData;
using BlocksCore.Application.Abstratctions;
using BlocksCore.AutoMapper.Abstractions;
using BlocksCore.Data.Abstractions.Paging;
using Microsoft.Extensions.Logging;


namespace Blocks.BussnessApplicationModule.TestAppService
{
	public class TestAppService : AppService, ITestAppService
	{
		private ILogger log;

		private MasterDataDomainEvent _masterDataDomain { get; set; }

		public TestAppService(MasterDataDomainEvent masterDataDomain,TestDomain testDomain, ILogger<TestAppService> log)
		{
			this.testDomain = testDomain;
			this.log = log;
			_masterDataDomain = masterDataDomain;
		}

		private TestDomain testDomain { get; set; }

		public string GetValue(string a)
		{
            log.LogInformation("123123");
			//log.Logger(new Framework.Logging.LogModel()
			//{
			//	Message = "123123"
			//});
			// encryptionUtility.Hash("123123123");
			return testDomain.GetValue(a);
		}

		public string GetValueOverride()
		{
			return testDomain.GetValueOverride();
		}

		public void Add(MasterDataInfo masterDataInfo)
		{
			var result = _masterDataDomain.Add(masterDataInfo.AutoMapTo<BussnessDomainModule.MasterData.MasterData>());
		}

		public List<string> ProxFunction(BussnessDTOModule.MasterData.ProxModel input)
		{
			testDomain.Update(null);
			return input.dic.Select(t => t.Key).ToList();
		}

		public object GetData(SearchModel page)
		{
			List<object> list = new List<object>();
			for (var i = 0; i < page.page.pageSize; i++)
			{
				list.Add(new
				{
					id = (page.page.page - 1) * page.page.pageSize + i
				});
			}
			return list;
		}

		public PageList<PageResult> GetServerData(SearchModel page)
		{
			return _masterDataDomain.GetPageList(page);

			//List<TestData> list = new List<TestData>();
			//var data = new List<TestData>();
			//for (var i = 0; i < 61; i++)
			//{
			//	data.Add(new TestData
			//	{
			//		ID = (i + 1).ToString(),
			//		name = "名字：" + (i + 1).ToString(),
			//		age = i + 20,
			//		theDate = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd HH:mm:ss"),
			//		theSelect = "1"
			//	});
			//}
			//if (page.page.pageSize == -1)
			//{
			//	list = data;
			//}
			//else
			//{
			//	if (page.page.sortOrder != null && page.page.sortColumn != null)
			//	{
			//		if (page.page.sortOrder == "asc")
			//		{
			//			data = data.OrderBy(n => n.ID).ToList();
			//		}
			//		else
			//		{
			//			data = data.OrderByDescending(n => n.ID).ToList();
			//		}
			//	}
			//	if (page.page.filters != null)
			//	{
			//		data = data.Where(n => n.ID.Contains(page.page.filters.rules[0].data)).ToList();
			//	}
			//	if (page.page.page * page.page.pageSize > data.Count)
			//	{
			//		for (var i = 0; i < data.Count - (page.page.page - 1) * page.page.pageSize; i++)
			//		{
			//			list.Add(data[((page.page.page - 1) * page.page.pageSize) + i]);
			//		}
			//	}
			//	else
			//	{
			//		for (var i = 0; i < page.page.pageSize; i++)
			//		{
			//			list.Add(data[((page.page.page - 1) * page.page.pageSize) + i]);
			//		}
			//	}
			//}
			//var pagerInfo = new
			//{
			//	EndIndex = 10,
			//	StartIndex = 1,
			//	page.page.page,
			//	page.page.pageSize,
			//	records = data.Count,
			//	total = data.Count % page.page.pageSize == 0 ? (data.Count / page.page.pageSize) : ((data.Count / page.page.pageSize) + 1)
			//};
			//return new
			//{
			//	pagerInfo,
			//	rows = list
			//};
		}

		public object GetClientData(SearchModel page)
		{
			List<object> list = new List<object>();
			for (var i = 0; i < 101; i++)
			{
				list.Add(new
				{
					Id = (i + 1).ToString(),
					name = "名字：" + (i + 1).ToString(),
					age = i + 20,
					theDate = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd HH:mm:ss"),
					theSelect = "1"
				});
			}
			var pagerInfo = new
			{
				EndIndex = 10,
				StartIndex = 1,
				page.page.page,
				page.page.pageSize,
				records = list.Count,
				total = list.Count % page.page.pageSize == 0 ? (list.Count / page.page.pageSize) : ((list.Count / page.page.pageSize) + 1)
			};
			return new
			{
				pagerInfo,
				rows = list
			};
		}
	}
}