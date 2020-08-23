using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.UnitOfWork;

using BlocksCore.Data.Linq2DB.Repository;

namespace SysMgt.BussnessRespositoryModule
{
  public  class BdtaQueueRepository : DBSqlRepositoryBase<BDTA_QUEUE>, IBdtaQueueRepository
    {
        public BdtaQueueRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {

        }
    }
}
