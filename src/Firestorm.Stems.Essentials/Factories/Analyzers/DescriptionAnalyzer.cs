using Firestorm.Stems.Definitions;
using Firestorm.Stems.Essentials.Factories.Factories;
using Firestorm.Stems.Fuel.Analysis;

namespace Firestorm.Stems.Essentials.Factories.Analyzers
{
    internal class DescriptionAnalyzer : IDefinitionAnalyzer<FieldDefinition>
    {
        public IStemsCoreServices Configuration { get; set; }

        public void Analyze<TItem>(EngineImplementations<TItem> implementations, FieldDefinition definition) 
            where TItem : class
        {
            if (definition.Description == null)
                return;

            var description = new AttributeFieldDescription(definition.Description);
            implementations.Descriptions.Add(definition.FieldName, description);
        }
    }
}