using System;
using System.Collections.Generic;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Roots;
using Reflectious;

namespace Firestorm.Stems
{
    internal class AnalyzerCacheBuilder
    {
        private readonly AnalyzerCache _analyzerCache;
        private readonly IStemConfiguration _stemConfiguration;

        internal AnalyzerCacheBuilder(IStemConfiguration stemConfiguration)
        {
            _stemConfiguration = stemConfiguration;
            _analyzerCache = stemConfiguration.AnalyzerCache as AnalyzerCache;

            // TODO refactor here - maybe put LoadType in the interface?
            if(_analyzerCache == null)
                throw new StemStartSetupException("Configuration must contain an AnalyzerCache property of type AnalyzerCache.");
        }

        public void AnalyzeAllStems(IEnumerable<Type> stemTypes)
        {
            foreach (Type stemType in stemTypes)
            {
                AnalyzerStemType(stemType);
            }
        }

        private void AnalyzerStemType(Type type)
        {
            _analyzerCache.LoadAnalyzer<AttributeAnalyzer>(type, _stemConfiguration);

            Type itemType = GetStemItemType(type);
            //AnalyzerCache.LoadAnalyzer(typeof(FieldDefinitionAnalyzer<>).MakeGenericType(itemType), type, stemConfiguration);
        }

        private static Type GetStemItemType(Type type)
        {
            var stemType = type.GetGenericSubclass(typeof(Stem<>));

            if(stemType == null)
                throw new StemStartSetupException("Stem classes");

            var itemType = stemType.GetGenericArguments()[0];
            return itemType;
        }
    }
}