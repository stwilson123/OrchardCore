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
    public class PrintServiceRepository:DBSqlRepositoryBase<PRINT_SERVICE>, IPrintServiceRepository
    {
        public PrintServiceRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }
    }
}
