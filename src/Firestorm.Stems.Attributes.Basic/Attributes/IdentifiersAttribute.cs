using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Attributes.Basic.Resolvers;
using JetBrains.Annotations;

namespace Firestorm.Stems.Attributes.Basic.Attributes
{
    [MeansImplicitUse]
    public class IdentifiersAttribute : IdentifierAttribute
    {
        public IdentifiersAttribute()
        {
        }

        public IdentifiersAttribute(string name)
        {
            Name = name;
        }

        public override IAttributeResolver GetResolver()
        {
            return new IdentifierAttributeResolver(true);
        }
    }
}