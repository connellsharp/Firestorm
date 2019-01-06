using Firestorm.Engine.Fields;
using Firestorm.Stems.Attributes.Definitions;

namespace Firestorm.Stems.Fuel.Essential.Factories
{
    internal class AttributeFieldDescription : IFieldDescription
    {
        private readonly FieldDescription _description;

        internal AttributeFieldDescription(FieldDescription description)
        {
            _description = description;
        }

        public string Description => _description.Text;

        public object Example => _description.Example;
    }
}