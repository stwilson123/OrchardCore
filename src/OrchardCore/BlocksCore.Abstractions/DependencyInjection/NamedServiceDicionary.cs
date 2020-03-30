using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BlocksCore.SyntacticAbstractions.Collection;
using BlocksCore.SyntacticAbstractions.Types;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Abstractions.DependencyInjection
{
    internal class NamedServiceDicionary
    {
        
        private LazyConcurrentDictionary<KeyValuePair<string, Type>, ConcurrentDictionary<Type,Type>> Types;
        public NamedServiceDicionary()
        {
            Types = new LazyConcurrentDictionary<KeyValuePair<string, Type>, ConcurrentDictionary<Type,Type>>();
        }

        public void Add(KeyValuePair<string, Type> Key,Type value)
        {
            var lazyDic = Types.GetOrAdd(Key, (key) => new ConcurrentDictionary<Type, Type> ());

            lazyDic.GetOrAdd(value, value);
        }
       
        
        public IList<Type> Get(KeyValuePair<string, Type> Key)
        {
            var lazyDic = Types.GetOrAdd(Key, (key) => new ConcurrentDictionary<Type, Type> ());


            return lazyDic.Select(t => t.Value).ToList();
        }

        public ICollection<KeyValuePair<string, Type>> GetKeys()
        {
            return Types.Keys;
        }
    }

  
}