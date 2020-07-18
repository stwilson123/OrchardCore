using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.Infrastructure;

namespace BlocksCore.Data.Core.Infrastructure
{
    public class EntityModel : IModel
    {


        IEnumerable<Type> entityTypes;

        public EntityModel(IEnumerable<Type> entityTypes)
        {
            this.entityTypes = entityTypes;
        }

        public IEnumerable<Type> GetEntityTypes()
        {
            return entityTypes;
        }
    }
}
