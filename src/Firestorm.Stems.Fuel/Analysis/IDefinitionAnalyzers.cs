using System.Collections.Generic;
using Firestorm.Stems.Definitions;

namespace Firestorm.Stems.Fuel.Resolving.Analysis
{
    public interface IDefinitionAnalyzers
    {
        IEnumerable<IDefinitionAnalyzer<FieldDefinition>> FieldAnalyzers { get; }
        
        IEnumerable<IDefinitionAnalyzer<IdentifierDefinition>> IdentifierAnalyzers { get; }
    }
}