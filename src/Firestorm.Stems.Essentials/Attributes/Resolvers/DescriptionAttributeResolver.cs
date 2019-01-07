using System.Reflection;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;

namespace Firestorm.Stems.Essentials.Resolvers
{
    internal class DescriptionAttributeResolver : AttributeResolverBase
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