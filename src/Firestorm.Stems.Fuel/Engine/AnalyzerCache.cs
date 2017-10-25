using System;
using System.Collections.Generic;
using Firestorm.Stems.Attributes.Analysis;

namespace Firestorm.Stems.Fuel
{
    /// <summary>
    /// Resolves <see cref="IAnalyzer"/> instances using an internal cache for a supplied analyzer type and stem Type.
    /// </summary>
    public static class AnalyzerCache
    {
        private struct CacheKey
        {
            public Type AnalyzerType;
            public Type StemType;
        }

        private static readonly Dictionary<CacheKey, IAnalyzer> ResolverCache = new Dictionary<CacheKey, IAnalyzer>();
        private static readonly object LockObject = new object();

        public static TAnalyzer GetAnalyzer<TAnalyzer>(Type stemType, IStemConfiguration configuration, bool useCache)
            where TAnalyzer : IAnalyzer, new()
        {
            if (useCache)
                return GetAnalyzer<TAnalyzer>(stemType, configuration);
            else
                return CreateAnalyzer<TAnalyzer>(stemType, configuration);
        }

        public static TAnalyzer GetAnalyzer<TAnalyzer>(Type stemType, IStemConfiguration configuration)
            where TAnalyzer : IAnalyzer, new()
        {
            var cacheKey = new CacheKey
            {
                AnalyzerType = typeof(TAnalyzer),
                StemType = stemType
            };

            if (!ResolverCache.TryGetValue(cacheKey, out IAnalyzer analyzer))
            {
                analyzer = AddAnalyzer<TAnalyzer>(cacheKey, stemType, configuration);
            }

            var typedMappingResolver = (TAnalyzer)analyzer;
            if(typedMappingResolver == null)
                throw new InvalidCastException("Incorrect stemType argument given for attributeAnalyzer.");

            return typedMappingResolver;
        }

        public static void LoadAnalyzer<TAnalyzer>(Type stemType, IStemConfiguration configuration)
            where TAnalyzer : IAnalyzer, new()
        {
            var cacheKey = new CacheKey
            {
                AnalyzerType = typeof(TAnalyzer),
                StemType = stemType
            };

            AddAnalyzer<TAnalyzer>(cacheKey, stemType, configuration);
        }

        private static IAnalyzer AddAnalyzer<TAnalyzer>(CacheKey cacheKey, Type stemType, IStemConfiguration configuration)
            where TAnalyzer : IAnalyzer, new()
        {
            lock (LockObject)
            {
                if (!ResolverCache.TryGetValue(cacheKey, out IAnalyzer analyzer))
                {
                    analyzer = CreateAnalyzer<TAnalyzer>(stemType, configuration);
                    ResolverCache.Add(cacheKey, analyzer);
                }

                return analyzer;
            }
        }

        private static TAnalyzer CreateAnalyzer<TAnalyzer>(Type type, IStemConfiguration configuration)
            where TAnalyzer : IAnalyzer, new()
        {
            var analyzer = new TAnalyzer();
            analyzer.Analyze(type, configuration);
            return analyzer;
        }
    }
}