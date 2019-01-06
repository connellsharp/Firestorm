using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Attributes.Basic.Resolvers;

namespace Firestorm.Stems.Attributes.Basic.Attributes
{
    public class DescriptionAttribute : FieldAttribute
    {
        public DescriptionAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }

        public object Example { get; set; }

        public override IAttributeResolver GetResolver()
        {
            return new DescriptionAttributeResolver();
        }
    }
}