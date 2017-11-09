using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Attributes.Attributes;
using Firestorm.Stems.Attributes.Basic.Resolvers;
using JetBrains.Annotations;

namespace Firestorm.Stems.Attributes.Basic.Attributes
{
    [MeansImplicitUse]
    public class IdentifierAttribute : StemAttribute
    {
        public string Name { get; set; }

        public string Exactly { get; set; }

        public string GetDefaultName(string memberName)
        {
            return memberName.TrimStart("FindBy");
        }

        public override IAttributeResolver GetResolver()
        {
            return new IdentifierAttributeResolver(false);
        }
    }
}