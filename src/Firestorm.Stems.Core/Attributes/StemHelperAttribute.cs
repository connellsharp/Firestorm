using System;

namespace Firestorm.Stems
{
    /// <summary>
    /// Base class for a helper attribute, usually proving syntactic sugar for functionality in a <see cref="StemAttribute"/>.
    /// </summary>
    /// <remarks>
    /// This attribute itself is used as a marker and does not provide any functionality.
    /// </remarks>
    public abstract class StemHelperAttribute : Attribute
    {
    }
}