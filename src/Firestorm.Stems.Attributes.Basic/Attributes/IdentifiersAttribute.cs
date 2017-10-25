using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Basic.Resolvers;
using JetBrains.Annotations;

namespace Firestorm.Stems
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