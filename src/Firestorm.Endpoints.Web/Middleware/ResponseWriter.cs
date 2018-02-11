using System.Threading.Tasks;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Start
{
    internal class ResponseWriter
    {
        private readonly IHttpRequestResponder _responder;
        private readonly Response _response;

        public ResponseWriter(IHttpRequestResponder responder, Response response)
        {
            _responder = responder;
            _response = response;
        }

        public Task WriteAsync()
        {
            _responder.SetStatusCode(_response.StatusCode);

            foreach (var header in _response.Headers)
            {
                _responder.SetResponseHeader(header.Key, header.Value);
            }

            return _responder.SetResponseBodyAsync(_response.GetFullBody());
        }
    }
}