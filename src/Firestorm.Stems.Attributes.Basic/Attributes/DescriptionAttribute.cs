using System;
using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Basic.Resolvers;

namespace Firestorm.Stems
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