using System;
using Firestorm.Stems.Attributes.Analysis;
using JetBrains.Annotations;

namespace Firestorm.Stems.Attributes.Attributes
{
    /// <summary>
    /// Base class for an attribute that allows a member to be used by Firestorm Stems.
    /// </summary>
    [MeansImplicitUse]
    public abstract class StemAttribute : Attribute
    {
        public abstract IAttributeResolver GetResolver();
    }
}