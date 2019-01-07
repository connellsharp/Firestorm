using System;
using System.Collections.Generic;

namespace Firestorm.Stems.Analysis
{
    /// <summary>
    /// Resolves <see cref="IAnalyzer"/> instances using an internal cache for a supplied analyzer type.
    /// </summary>
    public class AnalyzerCache : IAnalyzerFactory
    {
        private struct CacheKey
        {
            public Type AnalyzerType;
            public Type StemType;
        }

        private readonly Dictionary<CacheKey, IAnalyzer> _resolverCache = new Dictionary<CacheKey, IAnalyzer>();
        private readonly object _lockObject = new object();
        private readonly AnalyzerCreator _creatingCreator = new AnalyzerCreator();

        public TAnalyzer GetAnalyzer<TAnalyzer>(Type stemType, IStemConfiguration configuration, bool useCache)
            where TAnalyzer : IAnalyzer, new()
        {
            if (useCache)
                return GetAnalyzer<TAnalyzer>(stemType, configuration);
            else
                return _creatingCreator.GetAnalyzer<TAnalyzer>(stemType, configuration);
        }

        public TAnalyzer GetAnalyzer<TAnalyzer>(Type stemType, IStemConfiguration configuration)
            where TAnalyzer : IAnalyzer, new()
        {
            var cacheKey = new CacheKey
            {
                AnalyzerType = typeof(TAnalyzer),
                StemType = stemType
            };

            if (!_resolverCache.TryGetValue(cacheKey, out IAnalyzer analyzer))
            {
                analyzer = AddAnalyzer<TAnalyzer>(cacheKey, stemType, configuration);
            }

            var typedMappingResolver = (TAnalyzer) analyzer;
            if (typedMappingResolver == null)
                throw new InvalidCastException("Incorrect stemType argument given for attributeAnalyzer.");

            return typedMappingResolver;
        }

        public void LoadAnalyzer<TAnalyzer>(Type stemType, IStemConfiguration configuration)
            where TAnalyzer : IAnalyzer, new()
        {
            var cacheKey = new CacheKey
            {
                AnalyzerType = typeof(TAnalyzer),
                StemType = stemType
            };

            AddAnalyzer<TAnalyzer>(cacheKey, stemType, configuration);
        }

        private IAnalyzer AddAnalyzer<TAnalyzer>(CacheKey cacheKey, Type stemType, IStemConfiguration configuration)
            where TAnalyzer : IAnalyzer, new()
        {
            lock (_lockObject)
            {
                if (!_resolverCache.TryGetValue(cacheKey, out IAnalyzer analyzer))
                {
                    analyzer = _creatingCreator.GetAnalyzer<TAnalyzer>(stemType, configuration);
                    _resolverCache.Add(cacheKey, analyzer);
                }

                return analyzer;
            }
        }
    }
}