using System;
using System.Collections.Concurrent;
using System.Threading;

namespace BlocksCore.SyntacticAbstractions.Types.Collections
{
    public class LazyConcurrentDictionary<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, Lazy<TValue>> concurrentDictionary;

        public LazyConcurrentDictionary()
        {
            this.concurrentDictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>();
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            var lazyResult = this.concurrentDictionary.GetOrAdd(key, k => new Lazy<TValue>(() => valueFactory(k), LazyThreadSafetyMode.PublicationOnly));

            return lazyResult.Value;
        }
        
        public TValue AddOrUpdate(TKey key,Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
        {
            var lazyResult = this.concurrentDictionary.AddOrUpdate(key, 
                k => new Lazy<TValue>(() => addValueFactory(k)),
                (k,lazyV) => new Lazy<TValue>(() =>
                {
                    var lastValue = default(TValue);
                    try
                    {
                        lastValue = lazyV.Value;

                    }
                    catch 
                    {
                        
                    }
                    return updateValueFactory(k, lastValue);
                     
                }));

            return lazyResult.Value;
        }
    }
}