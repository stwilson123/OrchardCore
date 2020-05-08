using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BlocksCore.Data.EF.DBContext
{
    public interface IDbContextProvider
    {
        TDbContext GetDbContext<TDbContext,TEntity>() where TDbContext : DbContext;



    }
}
