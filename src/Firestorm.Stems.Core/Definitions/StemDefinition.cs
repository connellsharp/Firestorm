namespace Firestorm.Stems.Definitions
{
    public class StemDefinition
    {
        public NamedDictionary<FieldDefinition> FieldDefinitions { get; } =
            new NamedDictionary<FieldDefinition>(n => new FieldDefinition { FieldName = n });
        
        public NamedDictionary<IdentifierDefinition> IdentifierDefinitions { get; } =
            new NamedDictionary<IdentifierDefinition>(n => new IdentifierDefinition { IdentifierName = n });

        public FieldDescription Description { get; set; }
    }
}