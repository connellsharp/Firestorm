using Firestorm.Core.Web;
using Firestorm.Endpoints.Preconditions;

namespace Firestorm.Endpoints.Start
{
    /// <summary>
    /// Reads the Firestorm API request from the client. Used by the <see cref="EndpointInvoker"/>.
    /// </summary>
    public interface IRequestReader
    {
        string RequestMethod { get; }

        IPreconditions GetPreconditions();

        ResourceBody GetRequestBody();
    }
}