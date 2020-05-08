using Microsoft.EntityFrameworkCore;

namespace BlocksCore.Data.EF.DBContext
{
    public class UnitOfWorkDbContextProvider : IDbContextProvider
    {
        public TDbContext GetDbContext<TDbContext, TEntity>() where TDbContext : DbContext
        {
            throw new System.NotImplementedException();
        }
    }
}