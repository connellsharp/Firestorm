using Firestorm.Stems.Definitions;
using Firestorm.Stems.Essentials.Factories.Factories;
using Firestorm.Stems.Fuel.Analysis;

namespace Firestorm.Stems.Essentials.Factories.Analyzers
{
    internal class IdentifierAnalyzer : IDefinitionAnalyzer<IdentifierDefinition>
    {
        public void Analyze<TItem>(EngineImplementations<TItem> implementations, IdentifierDefinition definition) 
            where TItem : class
        {
            var factory = new IdentifierFactory<TItem>(definition);
            implementations.IdentifierFactories.Add(definition.IdentifierName, factory);
        }
    }
}