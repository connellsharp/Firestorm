using Firestorm.Host;
using JetBrains.Annotations;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// The context for the request to an <see cref="IRestEndpoint"/>.
    /// </summary>
    public interface IEndpointContext
    {
        /// <summary>
        /// The global configuration for all endpoints in this API.
        /// </summary>
        [NotNull]
        RestEndpointConfiguration Configuration { get; }

        /// <summary>
        /// An object containing information about the request.
        /// </summary>
        IRequestContext Request { get; }
    }
}