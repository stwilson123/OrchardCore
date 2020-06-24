using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Repository;
using SysMgt.BussnessDTOModule.SysProgram;
using BlocksCore.Data.EF.Linq;
using BlocksCore.Data.Abstractions.UnitOfWork;

namespace SysMgt.BussnessRespositoryModule
{
	public class SysProgramRepository : DBSqlRepositoryBase<SYS_PROGRAM>, ISysProgramRepository
	{
		public SysProgramRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
		{
		}

		public PageList<SysProgramPageResult> GetPageList(SysProgramSearchModel search)
		{
			var sysProgramContextTable = GetContextTable();
			if (search != null)
			{
				if (!string.IsNullOrEmpty(search.ProgramParent))
				{
					sysProgramContextTable = sysProgramContextTable.Where((SYS_PROGRAM sysProgram) => sysProgram.PROGRAMPARENT == search.ProgramParent && sysProgram.PLATFORM == search.Platform);
				}
				else
				{
					sysProgramContextTable = sysProgramContextTable.Where((SYS_PROGRAM sysProgram) => (sysProgram.PROGRAMPARENT == null || sysProgram.PROGRAMPARENT == "") && sysProgram.PLATFORM == search.Platform);
				}
			}
			return sysProgramContextTable.OrderBy(x => x.PROGRAMEXTEND).Paging((SYS_PROGRAM sysProgram) => new SysProgramPageResult()
			{
				ID = sysProgram.Id,
				Name = sysProgram.PROGRAMNAME,
				Code = sysProgram.PROGRAMCODE,
				URL = sysProgram.PROGRAMPROPERTY,
				Sort = sysProgram.PROGRAMEXTEND
			}, search.page);
		}
	}
}