using System;
using System.Collections.Generic;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using Firestorm.Stems.Roots;
using Reflectious;

namespace Firestorm.Stems
{
    /// <summary>
    /// The overall analyzer that builds implementations for all stem types by using other analyzers.
    /// </summary>
    public class SurpremeAnalyzer : IAnalyzer<ImplementationCache, IEnumerable<Type>>
    {
        private readonly IEnumerable<IFieldDefinitionAnalyzer> _analyzers;

        public SurpremeAnalyzer(IEnumerable<IFieldDefinitionAnalyzer> analyzers)
        {
            _analyzers = analyzers;
        }
        
        public void Analyze(ImplementationCache implementationCache, IEnumerable<Type> stemTypes)
        {
            foreach (Type stemType in stemTypes)
            {
                Type itemType = GetStemItemType(stemType);
                
                Reflect.Instance(this).GetMethod(nameof(AnalyzeInternal))
                    .MakeGeneric(itemType)
                    .Invoke(implementationCache, stemType);
            }
        }

        private void AnalyzeInternal<TItem>(ImplementationCache implementationCache, Type stemType) 
            where TItem : class
        {
            var stemDefinition = new StemDefinition();

            var attributeAnalyzer = new AttributeAnalyzer();
            attributeAnalyzer.Analyze(stemDefinition, stemType);

            var implementations = new EngineImplementations<TItem>();
            var definitionAnalyzer = new DefinitionAnalyzer<TItem>(_analyzers);
            definitionAnalyzer.Analyze(implementations, stemDefinition);

            implementationCache.Add(stemType, implementations);
        }

        private static Type GetStemItemType(Type stemType)
        {
            Type stemGenericBaseType = stemType.GetGenericSubclass(typeof(Stem<>));

            if(stemGenericBaseType == null)
                throw new StemStartSetupException("Stem classes must derive from Stem<>.");

            Type itemType = stemGenericBaseType.GetGenericArguments()[0];
            return itemType;
        }
    }
}