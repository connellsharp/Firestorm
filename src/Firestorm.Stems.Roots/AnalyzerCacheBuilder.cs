using System;
using System.Collections.Generic;
using Firestorm.Stems.Attributes;
using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Fuel;

namespace Firestorm.Stems.Roots
{
    public static class AnalyzerCacheBuilder
    {
        public static void AnalyzeAllStems(IEnumerable<Type> stemTypes, IStemConfiguration stemConfiguration)
        {
            foreach (Type stemType in stemTypes)
            {
                AnalyzerStemType(stemType, stemConfiguration);
            }
        }

        private static void AnalyzerStemType(Type type, IStemConfiguration stemConfiguration)
        {
            AnalyzerCache.LoadAnalyzer<AttributeAnalyzer>(type, stemConfiguration);

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