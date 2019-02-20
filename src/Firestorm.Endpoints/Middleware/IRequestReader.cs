using Firestorm.Endpoints.Executors;
using Firestorm.Host.Infrastructure;
using Firestorm.Rest.Web;
using JetBrains.Annotations;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// Reads the Firestorm API request from the client. Used by the <see cref="EndpointExecutor"/>.
    /// </summary>
    public interface IRequestReader
    {
        string RequestMethod { get; }

        IPreconditions GetPreconditions();

        [CanBeNull]
        ResourceBody GetRequestBody();

        IRestCollectionQuery GetQuery();
    }
}