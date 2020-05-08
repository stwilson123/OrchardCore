using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Domain.Abstractions
{
    public interface IEntity
    {

    }

    public interface IEntity<TPrimaryKey> : IEntity
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}
