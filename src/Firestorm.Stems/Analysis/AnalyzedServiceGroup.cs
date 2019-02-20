using System;
using System.Collections.Generic;

namespace Firestorm.Stems.Analysis
{
    public class AnalyzedServiceGroup : IServiceGroup
    {
        private readonly ServiceCache _cache;
        private readonly IAnalyzer<ServiceCache, IEnumerable<Type>> _analyzer;

        internal AnalyzedServiceGroup(IAnalyzer<ServiceCache, IEnumerable<Type>> analyzer)
        {
            _cache = new ServiceCache();
            _analyzer = analyzer;
        }

        public void Preload(IEnumerable<Type> stemTypes)
        {
            _analyzer.Analyze(_cache, stemTypes);
        }

        public IServiceProvider GetProvider(Type stemType)
        {
            if (!_cache.ContainsKey(stemType))
                _analyzer.Analyze(_cache, new[] {stemType});
            
            return new CacheServiceProvider(_cache.GetDictionary(stemType));
        }

        private class CacheServiceProvider : IServiceProvider
        {
            private readonly IDictionary<Type, object> _dictionary;

            public CacheServiceProvider(IDictionary<Type,object> dictionary)
            {
                _dictionary = dictionary;
            }

            public object GetService(Type serviceType)
            {
                return _dictionary[serviceType];
            }
        }
    }
}