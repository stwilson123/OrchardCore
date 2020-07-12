using System.Linq;

namespace BlocksCore.Domain.Abstractions
{
    public interface IDataContext
    {

        IQueryable Get<TEntity>() where TEntity : class;
    }
}