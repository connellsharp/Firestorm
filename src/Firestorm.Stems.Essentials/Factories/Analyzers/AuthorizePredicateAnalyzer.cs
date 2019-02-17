using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems.Essentials.Factories.Analyzers
{
    internal class AuthorizePredicateAnalyzer : IDefinitionAnalyzer<FieldDefinition>
    {
        public void Analyze<TItem>(EngineImplementations<TItem> implementations, FieldDefinition definition) 
            where TItem : class
        {
            if (definition.AuthorizePredicate == null)
                return;

            implementations.AuthorizePredicates.Add(definition.FieldName, definition.AuthorizePredicate);
        }
    }
}