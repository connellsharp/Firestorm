using System;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Essentials.Resolvers;

namespace Firestorm.Stems.Essentials
{
    public class SubstemAttribute : FieldAttribute
    {
        public SubstemAttribute(Type substemType)
        {
            SubstemType = substemType;
        }

        public Type SubstemType { get; private set; }

        public override Analysis.IAttributeResolver GetResolver()
        {
            return new SubstemAttributeResolver(SubstemType);
        }
    }
}