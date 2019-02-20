using System;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel.Analysis;

namespace Firestorm.Stems.Analysis
{
    /// <summary>
    /// Analyzes a <see cref="StemDefinition" /> object to build a <see cref="EngineImplementations{TItem}"/> model from a set of <see cref="IDefinitionAnalyzer<FieldDefinition>"/> implementations.
    /// </summary>
    internal class DefinitionAnalyzer<TItem> : IAnalyzer<EngineImplementations<TItem>, StemDefinition> 
        where TItem : class
    {
        private readonly IDefinitionAnalyzers _analyzers;

        public DefinitionAnalyzer(IDefinitionAnalyzers analyzers)
        {
            _analyzers = analyzers;
        }
        
        public void Analyze(EngineImplementations<TItem> implementations, StemDefinition stemDefinition)
        {
            foreach (FieldDefinition definition in stemDefinition.FieldDefinitions.Values)
            {
                foreach (IDefinitionAnalyzer<FieldDefinition> analyzer in _analyzers.FieldAnalyzers)
                {
                    try
                    {
                        analyzer.Analyze(implementations, definition);
                    }
                    catch (Exception ex)
                    {
                        throw new StemAttributeSetupException("Error setting up the '" + definition.FieldName + "' field with " + analyzer.GetType().Name + ".", ex);
                    }
                }
            }
            
            foreach (IdentifierDefinition definition in stemDefinition.IdentifierDefinitions.Values)
            {
                foreach (IDefinitionAnalyzer<IdentifierDefinition> analyzer in _analyzers.IdentifierAnalyzers)
                {
                    try
                    {
                        analyzer.Analyze(implementations, definition);
                    }
                    catch (Exception ex)
                    {
                        throw new StemAttributeSetupException("Error setting up the '" + definition.IdentifierName + "' identifier with " + analyzer.GetType().Name + ".", ex);
                    }
                }
            }
        }
    }
}