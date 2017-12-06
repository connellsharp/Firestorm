using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Start.Responses;

namespace Firestorm.Endpoints.Start
{
    /// <summary>
    /// Invokes the request from the given <see cref="IHttpRequestHandler"/> onto the given <see cref="IRestEndpoint"/>
    /// </summary>
    internal class EndpointInvoker
    {
        private readonly IEnumerable<IResponseBuilder> _builders;
        private readonly IHttpRequestHandler _requestHandler;
        private readonly IRestEndpoint _endpoint;

        public EndpointInvoker(IEnumerable<IResponseBuilder> builders, IHttpRequestHandler requestHandler, IRestEndpoint endpoint)
        {
            _builders = builders;
            _requestHandler = requestHandler;
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
            
            foreach (IResponseBuilder builder in _builders)
            {
                await builder.AddResourceAsync(_requestHandler, resourceBody);
            }
        }

        private async Task InvokeOptionsAsync()
        {
            Options options = await _endpoint.OptionsAsync();

            foreach (IResponseBuilder builder in _builders)
            {
                await builder.AddOptionsAsync(_requestHandler, options);
            }
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

            foreach (IResponseBuilder builder in _builders)
            {
                await builder.AddFeedbackAsync(_requestHandler, feedback);
            }
        }
    }
}