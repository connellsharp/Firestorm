using Firestorm.Core;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    /// <summary>
    /// Gets the objects to return in the response body from the return values from <see cref="IRestEndpoint"/> implementations.
    /// </summary>
    public interface IResponseContentGenerator
    {
        object GetFromResource(ResourceBody resource);

        object GetFromAcknowledgment(Acknowledgment acknowledgment);

        object GetFromError(ErrorInfo error, bool showDeveloperDetails);

        object GetFromOptions(Options options);
    }
}