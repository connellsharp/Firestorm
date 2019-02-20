using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using Firestorm.Stems.Roots;
using Reflectious;

namespace Firestorm.Stems.Analysis
{
    /// <summary>
    /// The overall analyzer that builds implementations for all stem types by using other analyzers.
    /// </summary>
    internal class SurpremeAnalyzer : IAnalyzer<ServiceCache, IEnumerable<Type>>
    {
        private readonly IPropertyAutoMapper _propertyAutoMapper;
        private readonly IDefinitionAnalyzers _analyzers;

        public SurpremeAnalyzer(IPropertyAutoMapper propertyAutoMapper, IDefinitionAnalyzers analyzers)
        {
            _propertyAutoMapper = propertyAutoMapper;
            _analyzers = analyzers;
        }
        
        public void Analyze(ServiceCache serviceCache, IEnumerable<Type> stemTypes)
        {
            foreach (Type stemType in stemTypes)
            {
                Type itemType = GetStemItemType(stemType);

                try
                {
                    Reflect.Instance(this).GetMethod(nameof(AnalyzeInternal))
                        .MakeGeneric(itemType)
                        .Invoke(serviceCache, stemType);
                }
                catch (TargetInvocationException ex)
                {
                    // TODO move to Reflectious
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    throw ex.InnerException;
                }
            }
        }

        private void AnalyzeInternal<TItem>(ServiceCache serviceCache, Type stemType) 
            where TItem : class
        {
            var stemDefinition = new StemDefinition();

            var attributeAnalyzer = new AttributeAnalyzer(_propertyAutoMapper);
            attributeAnalyzer.Analyze(stemDefinition, stemType);

            var implementations = new EngineImplementations<TItem>();
            
            var definitionAnalyzer = new DefinitionAnalyzer<TItem>(_analyzers);
            definitionAnalyzer.Analyze(implementations, stemDefinition);

            serviceCache.Add(stemType, implementations);
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