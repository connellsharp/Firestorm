using System;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Start
{
    /// <summary>
    /// Invokes the request from the given <see cref="IHttpRequestHandler"/> onto the given <see cref="IRestEndpoint"/>
    /// </summary>
    internal class EndpointInvoker
    {
        private readonly IHttpRequestHandler _requestHandler;
        private readonly IRestEndpoint _endpoint;
        private readonly ResponseWriter _responseWriter;

        public EndpointInvoker(IResponseBuilder responseBuilder, IHttpRequestHandler requestHandler, IRestEndpoint endpoint)
        {
            _requestHandler = requestHandler;
            _responseWriter = new ResponseWriter(_requestHandler, responseBuilder);
            _endpoint = endpoint;
        }

        public async Task InvokeAsync()
        {
            switch (_requestHandler.RequestMethod)
            {
                case "GET":
                    await InvokeGetAsync();
                    return;

                case "OPTIONS":
                    await InvokeOptionsAsync();
                    return;

                case "POST":
                case "PUT":
                case "PATCH":
                case "DELETE":
                    await InvokeUnsafeAsync();
                    return;

                default:
                    _requestHandler.SetStatusCode(HttpStatusCode.MethodNotAllowed);
                    return;
            }
        }

        private async Task InvokeGetAsync()
        {
            if (!_endpoint.EvaluatePreconditions(_requestHandler.GetPreconditions()))
            {
                _requestHandler.SetStatusCode(HttpStatusCode.NotModified);
                return;
            }

            ResourceBody resourceBody = await _endpoint.GetAsync();

            _responseWriter.AddResource(resourceBody);
            await _responseWriter.WriteAsync();
        }

        private async Task InvokeOptionsAsync()
        {
            Options options = await _endpoint.OptionsAsync();

            _responseWriter.AddOptions(options);
            await _responseWriter.WriteAsync();
        }

        private async Task InvokeUnsafeAsync()
        {
            if (!_endpoint.EvaluatePreconditions(_requestHandler.GetPreconditions()))
            {
                _requestHandler.SetStatusCode(HttpStatusCode.PreconditionFailed);
                return;
            }

            var method = (UnsafeMethod)Enum.Parse(typeof(UnsafeMethod), _requestHandler.RequestMethod, true);
            ResourceBody postBody = _requestHandler.GetRequestBodyObject();

            Feedback feedback = await _endpoint.UnsafeAsync(method, postBody);

            _responseWriter.AddFeedback(feedback);
            await _responseWriter.WriteAsync();
        }
    }
}