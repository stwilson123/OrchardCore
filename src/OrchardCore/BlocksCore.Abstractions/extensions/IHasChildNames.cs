using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Abstractions.Extensions
{
    public interface IHasChildNames
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string[] ChildNames { get; set; }
    }
}
