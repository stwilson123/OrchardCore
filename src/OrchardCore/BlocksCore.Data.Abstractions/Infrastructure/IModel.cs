using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Data.Abstractions.Infrastructure
{
    public interface IModel
    {
        IEnumerable<Type> GetEntityTypes();
    }
}
