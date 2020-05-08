using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BlocksCore.Data.EF.DBContext
{
    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
        {
            return context is BlocksDbContext blocksDbContext
             ? (context.GetType(), blocksDbContext.EntityTypes)
             : (object)context.GetType();
        }
    }
}
