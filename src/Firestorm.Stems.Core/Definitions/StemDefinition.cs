using System.Collections.Generic;

namespace Firestorm.Stems.Definitions
{
    public class StemDefinition
    {
        public FieldDefinitionDictionary FieldDefinitions { get; } = new FieldDefinitionDictionary();

        public Dictionary<string, IdentifierDefinition> IdentifierDefinitions { get; } = new Dictionary<string, IdentifierDefinition>();

        public FieldDescription Description { get; set; }
    }
}