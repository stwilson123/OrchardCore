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
    public class DictionaryTypeRepository : DBSqlRepositoryBase<BDTA_DICTIONARY_TYPE>, IDictionaryTypeRepository
    {
        public DictionaryTypeRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

    }
}
