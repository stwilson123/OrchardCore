using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessRespositoryModule.Public
{
    public interface IDeleteRepository
    {
        IList<Blocks.BussnessEntityModule.QueryEntity.DeleteReuslt> GetNotDeleteList(string Sqlstring);


        IList<Blocks.BussnessEntityModule.QueryEntity.DeleteIReuslt> ExeDelete(string Sqlstring);
    }
}
