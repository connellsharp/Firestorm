using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Fuel.Essential.Factories;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems.Fuel.Essential.Resolvers
{
    internal class DescriptionResolver : IFieldDefinitionResolver
    {
        public IStemConfiguration Configuration { get; set; }
        public FieldDefinition FieldDefinition { get; set; }

        public void IncludeDefinition<TItem>(EngineImplementations<TItem> implementations)
            where TItem : class
        {
            if (FieldDefinition.Description == null)
                return;

            var description = new AttributeFieldDescription(FieldDefinition.Description);
            implementations.Descriptions.Add(FieldDefinition.FieldName, description);
        }
    }
}