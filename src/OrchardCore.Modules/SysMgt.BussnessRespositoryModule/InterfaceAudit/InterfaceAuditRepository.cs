using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Repository;
using System;
using System.Collections.Generic;
using BlocksCore.Data.EF.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Collections.ObjectModel;
using SysMgt.BussnessDTOModule.InterfaceAudit;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Localization.Abtractions;

namespace SysMgt.BussnessRespositoryModule.InterfaceAudit
{
	public class InterfaceAuditRepository : DBSqlRepositoryBase<BLOCKS_AUDITLOGS>, IInterfaceAuditRepository
	{
		public Localizer L { get; set; }

		public InterfaceAuditRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
		{
		}

		public PageList<InterfaceAuditPageResult> GetPageList(InterfaceAuditSearchModel search)
		{
			return GetContextTable().OrderByDescending(n => n.CREATEDATE).Paging((BLOCKS_AUDITLOGS t) => new InterfaceAuditPageResult()
			{
				Id = t.Id,
				SysException = t.SYSTEMEXCEPTION,
				ExecutionTime = t.ExecutionTime,
				MethodName = t.MethodName,
				Creater = t.CREATER,
				Parameters = GetLJsonString(t.PARAMETERSDESCRIPTION),
				MethodDescription = L(t.METHODDESCRIPTION),
				OutParameters = GetLJsonString(t.OUTPARAMETERSDESCRIPTION),
				UserAccount = t.USERACCOUNT
			}, search.page);
		}

		private object GetLJsonString(string strParameters)
		{
			JObject jobj = GetVal(strParameters);
			return JsonConvert.SerializeObject(jobj);
		}

		private JObject GetVal(string json)
		{
			JObject jobj = new JObject();
			try
			{
				var entity = JObject.Parse(json);
				var props = entity.Properties();
				foreach (var item in props)
				{
					var value = item.Value;
					var name = item.Name;
					var type = item.Value.Type;
					if (type.ToString() == "Array")
					{
						List<object> newList = new List<object>();
						var list = JsonConvert.DeserializeObject<List<object>>(value.ToString());
						list.ForEach(n =>
						{
							if (n.GetType().Name == "String")
							{
								newList.Add(n.ToString());
							}
							else
							{
								newList.Add(GetVal(n.ToString()));
							}
						});
						jobj.Add(L(name), JToken.FromObject(newList));
					}
					else if (type.ToString() == "String")
					{
						jobj.Add(L(name), value.ToString());
					}
					else
					{
						if (item.Last.HasValues)
						{
							jobj.Add(L(name), GetVal(value.ToString()));
						}
						else
						{
							jobj.Add(L(name), value.ToString());
						}
					}
				}
			}
			catch 
			{
				jobj.Add(L("String"), json);
			}
			return jobj;
		}
	}
}
