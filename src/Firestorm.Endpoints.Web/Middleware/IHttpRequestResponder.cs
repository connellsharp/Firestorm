using System.Net;
using System.Threading.Tasks;

namespace Firestorm.Endpoints.Start
{
    public interface IHttpRequestResponder
    {
        void SetStatusCode(HttpStatusCode statusCode);

        Task SetResponseBodyAsync(object obj);

        void SetResponseHeader(string key, string value);
    }
}