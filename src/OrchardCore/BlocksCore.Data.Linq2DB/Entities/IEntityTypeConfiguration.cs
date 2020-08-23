using System;
using System.Collections.Generic;
using System.Text;
using LinqToDB.Mapping;

namespace BlocksCore.Data.Linq2DB.Entities
{
    public interface IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        void Configure(EntityMappingBuilder<TEntity> builder);
    }
}
