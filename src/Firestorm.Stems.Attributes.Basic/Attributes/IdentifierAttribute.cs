using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Basic.Resolvers;
using JetBrains.Annotations;

namespace Firestorm.Stems
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