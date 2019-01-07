using System;
using Firestorm.Stems.Analysis;
using JetBrains.Annotations;

namespace Firestorm.Stems
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