using System;

namespace Firestorm.Stems
{
    /// <summary>
    /// Base interface for <see cref="Stem"/>s and Roots.
    /// Used to pass context information down the stem chain.
    /// </summary>
    /// <remarks>
    /// Named based on the fact that Stems and Roots are "structural axes of a vascular plant".
    /// </remarks>
    public interface IAxis : IDisposable
    {
        IRestUser User { get; }

        IStemsCoreServices Services { get; }

        event EventHandler OnDispose;
    }
}