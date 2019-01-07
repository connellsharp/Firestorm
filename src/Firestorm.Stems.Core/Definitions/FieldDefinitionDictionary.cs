using System.Collections.Generic;

namespace Firestorm.Stems.Definitions
{
    public class FieldDefinitionDictionary : Dictionary<string, FieldDefinition>
    {

        public FieldDefinition GetOrCreate(string fieldName)
        {
            if (!TryGetValue(fieldName, out FieldDefinition definition))
            {
                definition = new FieldDefinition();
                definition.FieldName = fieldName;
                Add(fieldName, definition);
            }

            return definition;
        }
    }
}