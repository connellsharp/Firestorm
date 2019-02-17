using System;

namespace Firestorm
{
    /// <summary>
    /// An <see cref="IServiceProvider"/> capable of resolving requests for the current request.
    /// </summary>
    public interface IRequestServiceProvider : IServiceProvider
    {
    }
}