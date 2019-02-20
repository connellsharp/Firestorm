using Firestorm.Stems.Analysis;
using Firestorm.Stems.Essentials.Resolvers;
using JetBrains.Annotations;

namespace Firestorm.Stems.Essentials
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