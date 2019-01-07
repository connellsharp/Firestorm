using Firestorm.Stems.Analysis;
using Firestorm.Stems.Essentials.Resolvers;
using JetBrains.Annotations;

namespace Firestorm.Stems.Essentials
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