using Firestorm.Stems.Analysis;
using Firestorm.Stems.Essentials.Resolvers;

namespace Firestorm.Stems.Essentials
{
    public class DescriptionAttribute : FieldAttribute
    {
        public DescriptionAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }

        public object Example { get; set; }

        public override Analysis.IAttributeResolver GetResolver()
        {
            return new DescriptionAttributeResolver();
        }
    }
}