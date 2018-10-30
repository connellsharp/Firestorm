using System;
using JetBrains.Annotations;

namespace Firestorm.Host
{
    public interface IHostContext
    {
        /// <summary>
        /// The global configuration for all endpoints in this API.
        /// </summary>
        [NotNull]
        IServiceProvider Services { get; }
    }
}