using System.Reflection;
using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Attributes.Definitions;

namespace Firestorm.Stems.Basic.Resolvers
{
    public class DescriptionAttributeResolver : AttributeResolverBase
    {
        public override void IncludeMember(MemberInfo member)
        {
            if (!(Attribute is DescriptionAttribute descriptionAttribute))
                return;

            Definition.Description = new FieldDescription
            {
                Text = descriptionAttribute.Text,
                Example = descriptionAttribute.Example
            };
        }
    }
}