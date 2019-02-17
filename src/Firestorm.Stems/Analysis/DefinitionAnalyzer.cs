using System;
using System.Collections.Generic;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;

namespace Firestorm.Stems.Fuel.Resolving.Analysis
{
    /// <summary>
    /// Analyzes a <see cref="StemDefinition" /> object to build a <see cref="EngineImplementations{TItem}"/> model from a set of <see cref="IFieldDefinitionAnalyzer"/> implementations.
    /// </summary>
    public class DefinitionAnalyzer<TItem> : IAnalyzer<EngineImplementations<TItem>, StemDefinition> 
        where TItem : class
    {
        private readonly IEnumerable<IFieldDefinitionAnalyzer> _analyzers;

        public DefinitionAnalyzer(IEnumerable<IFieldDefinitionAnalyzer> analyzers)
        {
            _analyzers = analyzers;
        }
        
        public void Analyze(EngineImplementations<TItem> implementations, StemDefinition stemDefinition)
        {            
            foreach (FieldDefinition definition in stemDefinition.FieldDefinitions.Values)
            {
                foreach (IFieldDefinitionAnalyzer analyzer in _analyzers)
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
        }
    }
}