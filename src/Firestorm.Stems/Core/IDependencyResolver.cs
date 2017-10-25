using System;

namespace Firestorm.Stems
{
    /// <summary>
    /// Resolves dependencies for constructor parameters on Stem classes.
    /// </summary>
    public interface IDependencyResolver
    {
        object Resolve(Type type);
    }
}