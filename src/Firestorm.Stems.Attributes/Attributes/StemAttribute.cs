using System;
using Firestorm.Stems.Attributes.Analysis;
using JetBrains.Annotations;

namespace Firestorm.Stems
{
    [MeansImplicitUse]
    public abstract class StemAttribute : Attribute
    {
        public abstract IAttributeResolver GetResolver();
    }
}