using System;
using System.Collections.Generic;

namespace Firestorm.Stems
{
    public class ImplementationCache : IImplementationResolver
    {
        private readonly IDictionary<CacheKey, object> _dictionary;
        
        private struct CacheKey
        {
            public Type AnalyzerType;
            public Type StemType;
        }

        public ImplementationCache()
        {
            _dictionary = new Dictionary<CacheKey, object>();
        }

        public void Add<T>(Type stemType, T obj)
        {
            var key = new CacheKey
            {
                AnalyzerType = typeof(T),
                StemType = stemType
            };
            
            _dictionary.Add(key, obj);
        }

        public T Get<T>(Type stemType)
        {
            var key = new CacheKey
            {
                AnalyzerType = typeof(T),
                StemType = stemType
            };

            return (T) _dictionary[key];
        }
    }
}