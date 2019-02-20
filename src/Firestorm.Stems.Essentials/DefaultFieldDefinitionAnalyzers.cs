using System.Collections.Generic;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Essentials.Factories.Analyzers;
using Firestorm.Stems.Fuel.Analysis;

namespace Firestorm.Stems.Essentials
{
    public class DefaultFieldDefinitionAnalyzers : IDefinitionAnalyzers
    {
        public IEnumerable<IDefinitionAnalyzer<FieldDefinition>> FieldAnalyzers { get; } =
            new List<IDefinitionAnalyzer<FieldDefinition>>
            {
                new ExpressionOnlyDefinitionAnalyzer(),
                new RuntimeMethodDefinitionAnalyzer(),
                new AuthorizePredicateAnalyzer(),
                new DescriptionAnalyzer(),
                new DisplayForAnalyzer(),
                new SubstemDefinitionAnalyzer(),
            };

        public IEnumerable<IDefinitionAnalyzer<IdentifierDefinition>> IdentifierAnalyzers { get; } =
            new List<IDefinitionAnalyzer<IdentifierDefinition>>
            {
                new IdentifierAnalyzer()
            };
    }
}