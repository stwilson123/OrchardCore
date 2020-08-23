using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BlocksCore.Domain.Abstractions
{
    public interface IDataContext
    {

        IEnumerable<Type> EntityTypes { get; }

        IQueryable Get<TEntity>() where TEntity : class;

        IDbConnection  GetDbConnection();

        void ModelCreating();
    }
}