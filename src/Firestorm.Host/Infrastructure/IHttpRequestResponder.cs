using System.Net;

namespace Firestorm.Host.Infrastructure
{
    public interface IHttpRequestResponder
    {
        void SetStatusCode(HttpStatusCode statusCode);

        void SetResponseHeader(string key, string value);

        IContentAccepts GetAcceptHeaders();

        IContentWriter GetContentWriter();
    }
}