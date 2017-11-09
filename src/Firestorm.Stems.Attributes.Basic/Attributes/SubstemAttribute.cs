using System;
using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Attributes.Basic.Resolvers;

namespace Firestorm.Stems.Attributes.Basic.Attributes
{
    public class SubstemAttribute : FieldAttribute
    {
        public SubstemAttribute(Type substemType)
        {
            SubstemType = substemType;
        }

        public Type SubstemType { get; private set; }

        public override IAttributeResolver GetResolver()
        {
            return new SubstemAttributeResolver(SubstemType);
        }
    }
}