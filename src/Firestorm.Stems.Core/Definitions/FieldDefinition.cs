using System;

namespace Firestorm.Stems.Definitions
{
    /// <summary>
    /// Defines a field with weakly-typed expressions, delegates and types that can be used to build <see cref="IFactory{TFieldHandler,TItem}"/> objects.
    /// Definitions are built in a <see cref="FieldAttributeAnalyzer{TItem}"/>.
    /// </summary>
    public class FieldDefinition
    {
        public string FieldName { get; set; }

        public Type FieldType { get; set; }

        public Display? Display { get; set; }

        public FieldDefinitionHandlerPart Getter { get; } = new FieldDefinitionHandlerPart();

        public FieldDefinitionHandlerPart Setter { get; } = new FieldDefinitionHandlerPart();

        public FieldDefinitionHandlerPart Locator { get; } = new FieldDefinitionHandlerPart();

        public Type SubstemType { get; set; }

        public Func<IRestUser, bool> AuthorizePredicate { get; set; }

        public FieldDescription Description { get; set; }
    }
}