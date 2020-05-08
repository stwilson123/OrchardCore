using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Autofac.Extensions.DependencyInjection.Paramters
{
    public class NamedParam : Param
    {
        public NamedParam(string name,object value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; }

        public object Value { get; }
    }
}
