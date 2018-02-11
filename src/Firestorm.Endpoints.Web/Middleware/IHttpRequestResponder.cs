using System.Net;
using Firestorm.Endpoints.Formatting;

namespace Firestorm.Endpoints.Start
{
    public interface IHttpRequestResponder
    {
        void SetStatusCode(HttpStatusCode statusCode);

        void SetResponseHeader(string key, string value);

        IContentAccepts GetAcceptHeaders();

        IContentWriter GetContentWriter();
    }
}