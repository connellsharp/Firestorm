using Firestorm.Stems.Definitions;
using Firestorm.Stems.Essentials.Factories.Factories;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems.Essentials.Factories.Resolvers
{
    internal class DescriptionAnalyzer : IFieldDefinitionAnalyzer
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