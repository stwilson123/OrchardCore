using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.EF.Repository;


namespace SysMgt.BussnessRespositoryModule
{
  public  class UserVisitStatisticRepository : DBSqlRepositoryBase<SYS_USER_VISIT_STATISTIC>, IUserVisitStatisticRepository
    {
        public UserVisitStatisticRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
            
        }
      
    }
}
