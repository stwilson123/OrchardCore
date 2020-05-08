using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BlocksCore.Data.EF.Repository
{
    public interface IRepositoryWithDbContext
    {
        DbContext GetDbContext();
    }
}
