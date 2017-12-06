using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Preconditions;

namespace Firestorm.Endpoints.Start
{
    public interface IHttpRequestHandler
    {
        string RequestMethod { get; }

        string ResourcePath { get; }

        void SetStatusCode(HttpStatusCode statusCode);

        IPreconditions GetPreconditions();

        Task SetResponseBodyAsync(object obj);

        ResourceBody GetRequestBodyObject();

        void SetResponseHeader(string key, string value);
    }
}