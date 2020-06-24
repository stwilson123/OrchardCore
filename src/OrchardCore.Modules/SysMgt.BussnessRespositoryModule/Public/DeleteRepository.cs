using Blocks.BussnessEntityModule;
using BlocksCore.Data.EF.DBContext;
using BlocksCore.Data.Abstractions.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Data.EF.Repository;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Data.OracleClient;
using BlocksCore.Data.Abstractions.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace SysMgt.BussnessRespositoryModule.Public
{
   public class DeleteRepository : DBSqlRepositoryBase<BDTA_MATERIAL>, IDeleteRepository
    {
        public DeleteRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }
        public  IList<Blocks.BussnessEntityModule.QueryEntity.DeleteReuslt> GetNotDeleteList(string Sqlstring)
        {
            return this.SqlQuery<Blocks.BussnessEntityModule.QueryEntity.DeleteReuslt>(Sqlstring);

        }

        public IList<Blocks.BussnessEntityModule.QueryEntity.DeleteIReuslt>  ExeDelete(string Sqlstring)
        {

            return this.SqlQuery<Blocks.BussnessEntityModule.QueryEntity.DeleteIReuslt>(Sqlstring);

        }







    }
}
