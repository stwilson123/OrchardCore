using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.UnitOfWork;

using BlocksCore.Data.Linq2DB.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessRespositoryModule
{
    public class SysProgramOperationRepository : DBSqlRepositoryBase<SYS_PROGRAMOPERATION>, ISysProgramOperationRepository
    {
        public SysProgramOperationRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }
    }
}
